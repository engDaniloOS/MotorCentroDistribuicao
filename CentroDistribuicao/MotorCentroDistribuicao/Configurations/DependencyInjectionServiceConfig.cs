using MotorCentroDistribuicao.Domain;
using MotorCentroDistribuicao.Domain.Providers.Repository;
using MotorCentroDistribuicao.Domain.Providers.Rest;
using MotorCentroDistribuicao.Domain.UseCases;
using MotorCentroDistribuicao.Providers;

namespace MotorCentroDistribuicao.Configurations
{
    public static class DependencyInjectionServiceConfig
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IProcessarPedidoUseCase, ProcessarPedidoUseCase>();

            services.AddScoped<ICentroDistribuicaoProvider, CentroDistribuicaoProvider>();

            services.AddSingleton<IPedidoRepository, PedidoMockRepository>();
        }
    }
}
