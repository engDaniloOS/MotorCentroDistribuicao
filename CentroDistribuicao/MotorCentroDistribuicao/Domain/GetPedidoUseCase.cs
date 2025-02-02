using AutoMapper;
using MotorCentroDistribuicao.Domain.Dtos;
using MotorCentroDistribuicao.Domain.Providers.Repository;
using MotorCentroDistribuicao.Domain.UseCases;

namespace MotorCentroDistribuicao.Domain
{
    public class GetPedidoUseCase(
        IPedidoRepository pedidoRepository,
        IMapper mapper) : IGetPedidoUseCase
    {
        public async Task<PedidoOutDto> GetPedidoProcessado(Guid pedidoId)
        {
            var pedido = await pedidoRepository.Get(pedidoId.ToString());

            return mapper.Map<PedidoOutDto>(pedido);
        }
    }
}
