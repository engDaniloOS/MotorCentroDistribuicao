using AutoMapper;
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
        IMapper mapper,
        ILogger<ProcessarPedidoUseCase> logger) : IProcessarPedidoUseCase
    {
        private const string MSG_NAO_ENCONTRADO = "não encontrado.";

        public async Task<PedidoOutDto> GetCentrosDistribuicao(PedidoDto pedido)
        {
            logger.LogInformation("Iniciando processamento do pedido", pedido);

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var itensParaProcessamento = pedido.Itens.Distinct().ToList();

            var itens =
                await CallCentroDistribuicaoProviderEmParalelo(itensParaProcessamento);

            var respostaPedido = buildPedidoOutDto(itens);

            if (respostaPedido.Id != Guid.Empty)
                await SalvarPedido(respostaPedido);

            logger.LogInformation($"Processamento realizado em {stopWatch.ElapsedMilliseconds}ms", respostaPedido);

            return respostaPedido;
        }

        private PedidoOutDto buildPedidoOutDto(List<ItemDto> itens)
        {
            var isProcessamentoItensOk =
                itens.All(item => string.IsNullOrWhiteSpace(item.Message));

            var itensNaoEncontrados =
                itens.All(item => item.Message.Contains(MSG_NAO_ENCONTRADO));

            var validadePedidoMin =
                int.Parse(configuration.GetRequiredSection("Pedidos")["ValidadeMin"]!);

            return new PedidoOutDto
            {
                Id = (isProcessamentoItensOk && !itensNaoEncontrados) ? Guid.NewGuid() : Guid.Empty,
                Itens = itens,
                Validade = DateTime.Now.AddMinutes(validadePedidoMin),
                HasError = !isProcessamentoItensOk,
                ErrorMessage = isProcessamentoItensOk ? string.Empty : "Erro ao processar itens",
                NotFound = itensNaoEncontrados
            };
        }

        private async Task SalvarPedido(PedidoOutDto pedidoOutDto)
        {
            try
            {
                var modeloPedido = mapper.Map<Pedido>(pedidoOutDto);
                await pedidoRepository.Salvar(modeloPedido);
            }
            catch (Exception ex)
            {
                logger.LogError($"Erro ao salvar o pedido {pedidoOutDto.Id} na base de dados. Erro: {ex.Message}");
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
                catch(KeyNotFoundException)
                {
                    var error = $"Item {item} não encontrado.";
                    logger.LogError(error);

                    itensProcessados.Add(new ItemDto { Id = item, Message = error });
                }
                catch(Exception ex)
                {
                    logger.LogError($"Erro ao processar o item {item}. Erro: {ex.Message}");

                    itensProcessados.Add(new ItemDto { Id = item, Message = $"Não foi possível processar o item" });
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
