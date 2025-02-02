using MotorCentroDistribuicao.Domain.Models;

namespace MotorCentroDistribuicao.Domain.Providers.Repository
{
    public interface IPedidoRepository
    {
        Task Salvar(Pedido pedido);
        Task<Pedido> Get(string pedidoID);
    }
}
