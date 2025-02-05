using LiteDB;

namespace MotorCentroDistribuicao.Configurations
{
    public static class DatabaseServiceConfig
    {
        public static void Configure(IServiceCollection services)
        {
            var database = new LiteDatabase(":memory:");

            services.AddSingleton<ILiteDatabase>(database);
        }
    }
}
