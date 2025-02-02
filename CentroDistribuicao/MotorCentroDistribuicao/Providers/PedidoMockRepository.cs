using MotorCentroDistribuicao.Domain.Dtos;
using MotorCentroDistribuicao.Domain.Providers.Repository;

namespace MotorCentroDistribuicao.Providers
{
    public class PedidoMockRepository : IPedidoRepository
    {
        private static Dictionary<string, List<ItemDto>> baseDeDadosMock = [];

        public async Task<PedidoOutDto> Get(string pedidoID)
        {
            await Task.Delay(20);

            var findValue = baseDeDadosMock.TryGetValue(pedidoID, out var itens);

            if (findValue)
                return new PedidoOutDto { Id = Guid.Parse(pedidoID), Itens = itens };

            return new PedidoOutDto();
        }

        public async Task Salvar(Guid id, List<ItemDto> itens)
        {
            baseDeDadosMock.Add(id.ToString(), itens);

            await Task.Delay(10);
        }
    }
}
