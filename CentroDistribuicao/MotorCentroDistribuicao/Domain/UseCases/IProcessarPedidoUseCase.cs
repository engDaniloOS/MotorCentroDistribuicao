using MotorCentroDistribuicao.Domain.Dtos;

namespace MotorCentroDistribuicao.Domain.UseCases
{
    public interface IProcessarPedidoUseCase
    {
        Task<CentroDistribuicaoOutDto> GetCentrosDistribuicaoComItensVinculados(PedidoDto pedido);
        Task<ItemOutDto> GetCentrosDistribuicao(PedidoDto pedido);
    }
}
