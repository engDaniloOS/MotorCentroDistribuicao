using MotorCentroDistribuicao.Domain.Dtos;
using MotorCentroDistribuicao.Domain.Providers.Repository;
using MotorCentroDistribuicao.Domain.UseCases;

namespace MotorCentroDistribuicao.Domain
{
    public class GetPedidoUseCase(IPedidoRepository pedidoRepository) : IGetPedidoUseCase
    {
        public async Task<PedidoOutDto> GetPedidoProcessado(Guid pedidoId)
        {
            return await pedidoRepository.Get(pedidoId.ToString());
        }
    }
}
