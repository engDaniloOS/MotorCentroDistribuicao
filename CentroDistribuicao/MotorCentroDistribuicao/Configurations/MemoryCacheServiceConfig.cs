namespace MotorCentroDistribuicao.Configurations
{
    public static class MemoryCacheServiceConfig
    {
        public static void Configure(IServiceCollection services) => services.AddMemoryCache();
    }
}
