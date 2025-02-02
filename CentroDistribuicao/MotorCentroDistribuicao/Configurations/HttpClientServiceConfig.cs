namespace MotorCentroDistribuicao.Configurations
{
    public static class HttpClientServiceConfig
    {
        public static readonly string HTTP_CLIENT_CD = "centro_distribuicao";

        public static void Configure(IServiceCollection services, IConfiguration configurations)
        {
            var centroDistribuicaoUrl = configurations.GetRequiredSection("Http")["CentroDistribuicaoUrl"];

            services.AddHttpClient(HTTP_CLIENT_CD, client =>
            {
                client.BaseAddress = new Uri(centroDistribuicaoUrl!);
                client.Timeout = TimeSpan.FromSeconds(10);
            });
        }
    }
}
