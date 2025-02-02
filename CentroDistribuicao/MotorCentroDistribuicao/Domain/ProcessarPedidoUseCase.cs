using MotorCentroDistribuicao.Domain.Dtos;
using MotorCentroDistribuicao.Domain.Providers.Repository;
using MotorCentroDistribuicao.Domain.Providers.Rest;
using MotorCentroDistribuicao.Domain.UseCases;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace MotorCentroDistribuicao.Domain
{
    public class ProcessarPedidoUseCase(
        ICentroDistribuicaoProvider cdprovider,
        IPedidoRepository pedidoWriterRepository) : IProcessarPedidoUseCase
    {
        public async Task<CentroDistribuicaoOutDto> GetCentrosDistribuicaoComItensVinculados(PedidoDto pedido)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemOutDto> GetCentrosDistribuicao(PedidoDto pedido)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var itensParaProcessamento = RemoveItensDuplicados(pedido.Itens);

            var itens = new ConcurrentBag<ItemDto>();

            var semaforo = new SemaphoreSlim(8);

            await Parallel.ForEachAsync(itensParaProcessamento, async (item, cancellationToken) =>
            {
                await semaforo.WaitAsync(cancellationToken);

                try
                {
                    var respostaProvider = await cdprovider.GetCentrosDistribuicaoPorItem(item);

                    itens.Add(
                        new ItemDto 
                        {
                            Id = item, 
                            CentrosDistribuicao = respostaProvider.CentrosDistribuicao
                        });
                }
                finally
                {
                    semaforo.Release();
                }
            });

            var respostaPedido = new ItemOutDto { Itens = [.. itens] };

            await pedidoWriterRepository.Salvar(respostaPedido.Id, respostaPedido.Itens);

            Console.WriteLine($"Processamento realizado em {stopWatch.ElapsedMilliseconds}ms");

            return respostaPedido;
        }

        private List<long> RemoveItensDuplicados(List<long> itens) => itens.Distinct().ToList();
    }
}
