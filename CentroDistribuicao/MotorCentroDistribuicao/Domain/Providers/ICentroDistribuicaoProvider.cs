using MotorCentroDistribuicao.Domain.Providers.Dtos;

namespace MotorCentroDistribuicao.Domain.Providers
{
    public interface ICentroDistribuicaoProvider
    {
        Task<CentroDistribuicaoProviderDto> GetCentrosDistribuicaoPorItem(long item);
    }
}
