using AutoMapper;
using MotorCentroDistribuicao.Configurations;
using MotorCentroDistribuicao.Domain.Dtos;
using MotorCentroDistribuicao.Domain.Models;
using MotorCentroDistribuicao.Domain.Providers.Repository;
using MotorCentroDistribuicao.Domain.Providers.Rest;
using MotorCentroDistribuicao.Domain.UseCases;
using System.Collections.Concurrent;

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
            logger.LogInformation("Iniciando processamento do pedido");

            var itensParaProcessamento = pedido.Itens.Distinct().ToList();
            var itensProcessados = await ProcessarItensEmParalelo(itensParaProcessamento);
            var respostaPedido = BuildPedidoOutDto(itensProcessados);

            if (respostaPedido.Id != Guid.Empty) 
                SalvarPedido(respostaPedido);

            logger.LogInformation("Processamento finalizado");

            return respostaPedido;
        }

        private PedidoOutDto BuildPedidoOutDto(List<ItemDto> itens)
        {
            var isProcessamentoItensOk =
                itens.All(item => string.IsNullOrWhiteSpace(item.ErrorMessage));

            var itensNaoEncontrados =
                itens.All(item => item.ErrorMessage?.Contains(MSG_NAO_ENCONTRADO) == true);

            var validadePedidoMin =
                int.Parse(configuration.GetRequiredSection("Pedidos")["ValidadeMin"]!);

            return new PedidoOutDto
            {
                Id = (isProcessamentoItensOk && !itensNaoEncontrados) ? Guid.NewGuid() : Guid.Empty,
                Itens = itens,
                Validade = DateTime.Now.AddMinutes(validadePedidoMin),
                HasError = !isProcessamentoItensOk,
                ErrorMessage = isProcessamentoItensOk ? null : "Erro ao processar itens",
                NotFound = itensNaoEncontrados
            };
        }

        private void SalvarPedido(PedidoOutDto pedidoOutDto)
        {
            try
            {
                var modeloPedido = mapper.Map<Pedido>(pedidoOutDto);
                pedidoRepository.Salvar(modeloPedido);
            }
            catch (Exception ex)
            {
                logger.LogError($"Erro ao salvar o pedido {pedidoOutDto.Id} na base de dados. Erro: {ex.Message}");
            }
        }

        private async Task<List<ItemDto>> ProcessarItensEmParalelo(List<long> itens)
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
                            ErrorMessage = respostaProvider.CentrosDistribuicao.Any() ? null : "Item indisponível"
                        });
                }
                catch(KeyNotFoundException)
                {
                    var error = $"Item {item} não encontrado.";
                    logger.LogError(error);

                    itensProcessados.Add(new ItemDto { Id = item, ErrorMessage = error });
                }
                catch(Exception ex)
                {
                    logger.LogError($"Erro ao processar o item {item}. Erro: {ex.Message}");
                    itensProcessados.Add(new ItemDto { Id = item, ErrorMessage = $"Não foi possível processar o item" });
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
