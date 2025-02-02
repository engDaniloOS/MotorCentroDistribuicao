using MotorCentroDistribuicao.Domain.Dtos;
using MotorCentroDistribuicao.Domain.Providers;
using MotorCentroDistribuicao.Domain.Services;

namespace MotorCentroDistribuicao.Domain
{
    public class PedidoService(ICentroDistribuicaoProvider cdprovider) : IPedidosService
    {
        public async Task<CentroDistribuicaoOutDto> GetCentrosDistribuicaoComItensVinculados(PedidoDto pedido)
        {
            throw new NotImplementedException();
        }

        public async Task<ItemOutDto> GetItensComCentrosDistribuicaoVinculados(PedidoDto pedido)
        {
            var itens = new List<ItemDto>();

            foreach (var item in pedido.Itens)
            {
                var respostaProvider = await cdprovider.GetCentrosDistribuicaoPorItem(item);
                itens.Add(new ItemDto { Id = item, CentrosDistribuicao = respostaProvider.CentrosDistribuicao });
            }

            return new ItemOutDto { Itens = itens };
        }
    }
}
