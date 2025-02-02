using MotorCentroDistribuicao.Domain.Dtos;

namespace MotorCentroDistribuicao.Domain.UseCases
{
    public interface IGetPedidoUseCase
    {
        Task<PedidoOutDto> GetPedidoProcessado(Guid pedidoId);
    }
}
