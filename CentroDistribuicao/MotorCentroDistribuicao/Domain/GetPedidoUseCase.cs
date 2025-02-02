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
            try
            {
                var pedido = await pedidoRepository.Get(pedidoId.ToString());

                return mapper.Map<PedidoOutDto>(pedido);
            }
            catch (Exception ex)
            {
                var erroMessage = $"Erro ao buscar o pedido. Pedido {pedidoId}. Erro: {ex.Message}";
                Console.WriteLine(erroMessage);

                return new PedidoOutDto
                {
                    HasError = true,
                    ErrorMessage = erroMessage
                };
            }
        }
    }
}
