using MotorCentroDistribuicao.Domain.Dtos;

namespace MotorCentroDistribuicao.Domain.Services
{
    public interface IPedidosService
    {
        Task<CentroDistribuicaoOutDto> GetCentrosDistribuicaoComItensVinculados(PedidoDto pedido);
        Task<ItemOutDto> GetItensComCentrosDistribuicaoVinculados(PedidoDto pedido);
    }
}
