using LiteDB;
using MotorCentroDistribuicao.Domain.Models;
using MotorCentroDistribuicao.Domain.Providers.Repository;

namespace MotorCentroDistribuicao.Providers.Repositories
{
    public class PedidoRepository(ILiteDatabase database) : IPedidoRepository
    {
        public Pedido Get(Guid pedidoID)
        {
            var collection = database.GetCollection<Pedido>("pedidos");

            return collection.FindById(pedidoID);
        }

        public void Salvar(Pedido pedido)
        {
            var collection = database.GetCollection<Pedido>("pedidos");

            collection.Insert(pedido);
        }
    }
}
