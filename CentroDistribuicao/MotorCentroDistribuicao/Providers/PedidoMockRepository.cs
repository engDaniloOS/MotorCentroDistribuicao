using MotorCentroDistribuicao.Domain.Dtos;
using MotorCentroDistribuicao.Domain.Providers.Repository;

namespace MotorCentroDistribuicao.Providers
{
    public class PedidoMockRepository : IPedidoRepository
    {
        private static Dictionary<Guid, List<ItemDto>> baseDeDadosMock = [];


        public async Task Salvar(Guid id, List<ItemDto> itens)
        {
            baseDeDadosMock.Add(id, itens);

            await Task.Delay(10);
        }
    }
}
