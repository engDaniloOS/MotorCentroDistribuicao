using MotorCentroDistribuicao.Domain.Dtos;

namespace MotorCentroDistribuicao.Domain.Providers.Repository
{
    public interface IPedidoRepository
    {
        Task Salvar(Guid id, List<ItemDto> itens);
    }
}
