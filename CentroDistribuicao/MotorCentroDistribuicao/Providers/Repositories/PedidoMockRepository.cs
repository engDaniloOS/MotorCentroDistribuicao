using MotorCentroDistribuicao.Domain.Models;
using MotorCentroDistribuicao.Domain.Providers.Repository;

namespace MotorCentroDistribuicao.Providers.Repositories
{
    public class PedidoMockRepository : IPedidoRepository
    {
        private static Dictionary<string, Pedido> baseDeDadosMock = [];

        public async Task<Pedido> Get(string pedidoID)
        {
            await Task.Delay(20);

            var findValue = baseDeDadosMock.TryGetValue(pedidoID, out var pedido);

            if (findValue)
                return pedido!;

            return new Pedido();
        }

        public async Task Salvar(Pedido pedido)
        {
            baseDeDadosMock.Add(pedido.Id.ToString()!, pedido);

            await Task.Delay(20);
        }
    }
}
