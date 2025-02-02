using AutoMapper;
using Microsoft.Extensions.Configuration;
using MotorCentroDistribuicao.Configurations;
using MotorCentroDistribuicao.Domain.Dtos;
using MotorCentroDistribuicao.Domain.Models;
using MotorCentroDistribuicao.Domain.Providers.Repository;
using MotorCentroDistribuicao.Domain.Providers.Rest;
using MotorCentroDistribuicao.Domain.UseCases;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace MotorCentroDistribuicao.Domain
{
    public class ProcessarPedidoUseCase(
        ICentroDistribuicaoProvider cdprovider,
        IPedidoRepository pedidoRepository,
        IConfiguration configuration,
        IMapper mapper) : IProcessarPedidoUseCase
    {
        public async Task<PedidoOutDto> GetCentrosDistribuicao(PedidoDto pedido)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var itensParaProcessamento = RemoveItensDuplicados(pedido.Itens);

            var itens = await CallCentroDistribuicaoProviderEmParalelo(itensParaProcessamento);

            var validadePedidoMin = int.Parse(configuration.GetRequiredSection("Pedidos")["ValidadeMin"]);
            var respostaPedido = new PedidoOutDto
            {
                Id = Guid.NewGuid(),
                Itens = itens,
                Validade = DateTime.Now.AddMinutes(validadePedidoMin)
            };

            await SalvarPedido(respostaPedido);

            Console.WriteLine($"Processamento realizado em {stopWatch.ElapsedMilliseconds}ms");

            return respostaPedido;
        }

        private List<long> RemoveItensDuplicados(List<long> itens) 
            => itens.Distinct().ToList();

        private async Task SalvarPedido(PedidoOutDto pedidoOutDto)
        {
            try
            {
                var modeloPedido = mapper.Map<Pedido>(pedidoOutDto);
                await pedidoRepository.Salvar(modeloPedido);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar o pedido {pedidoOutDto.Id} na base de dados. Erro: {ex.Message}");
            }
        }

        private async Task<List<ItemDto>> CallCentroDistribuicaoProviderEmParalelo(List<long> itens)
        {
            var itensProcessados = new ConcurrentBag<ItemDto>();
            var semaforo = new SemaphoreSlim(HttpClientServiceConfig.MaxRequisicoesParalelas);

            await Parallel.ForEachAsync(itens, async (item, cancellationToken) =>
            {
                await semaforo.WaitAsync(cancellationToken);

                try
                {
                    var respostaProvider = await cdprovider.GetCentrosDistribuicaoPorItem(item);

                    itensProcessados.Add(
                        new ItemDto
                        {
                            Id = item,
                            CentrosDistribuicao = respostaProvider.CentrosDistribuicao,
                            Message = respostaProvider.CentrosDistribuicao.Any() ? string.Empty : "Item indisponível"
                        });
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Erro ao processar o item {item}. Erro: {ex.Message}");

                    itensProcessados.Add(
                        new ItemDto
                        {
                            Id = item,
                            Message = $"Não foi possível processar o item"
                        });
                }
                finally
                {
                    semaforo.Release();
                }
            });

            return [.. itensProcessados];
        }
    }
}
