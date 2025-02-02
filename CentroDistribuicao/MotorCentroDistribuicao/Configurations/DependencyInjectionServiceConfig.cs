using MotorCentroDistribuicao.Domain;
using MotorCentroDistribuicao.Domain.Providers;
using MotorCentroDistribuicao.Domain.Services;
using MotorCentroDistribuicao.Providers;

namespace MotorCentroDistribuicao.Configurations
{
    public static class DependencyInjectionServiceConfig
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IPedidosService, PedidoService>();

            services.AddScoped<ICentroDistribuicaoProvider, CentroDistribuicaoProvider>();
        }
    }
}
