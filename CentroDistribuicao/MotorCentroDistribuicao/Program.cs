
using MotorCentroDistribuicao.Configurations;

namespace MotorCentroDistribuicao
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            DependencyInjectionServiceConfig.Configure(builder.Services);
            HttpClientServiceConfig.Configure(builder.Services, builder.Configuration);
            MapperServiceConfig.Configure(builder.Services);
            MemoryCacheServiceConfig.Configure(builder.Services);
            LogServiceConfig.Configure();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
