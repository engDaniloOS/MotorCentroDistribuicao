using AutoMapper;
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
        IMapper mapper) : IProcessarPedidoUseCase
    {
        public async Task<PedidoOutDto> GetCentrosDistribuicao(PedidoDto pedido)
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

            var respostaPedido = new PedidoOutDto
            {
                Id = Guid.NewGuid(),
                Itens = [.. itens],
                Validade = DateTime.Now.AddMinutes(10)
            };

            var modeloPedido = mapper.Map<Pedido>(respostaPedido);

            await pedidoRepository.Salvar(modeloPedido);

            Console.WriteLine($"Processamento realizado em {stopWatch.ElapsedMilliseconds}ms");

            return respostaPedido;
        }

        private List<long> RemoveItensDuplicados(List<long> itens) => itens.Distinct().ToList();
    }
}
