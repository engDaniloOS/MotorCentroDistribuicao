namespace MotorCentroDistribuicao.Configurations
{
    public static class HttpClientServiceConfig
    {
        public static int MaxRequisicoesParalelas { get; private set; }

        public const string HTTP_CLIENT_CD = "centro_distribuicao";

        private const string RETRY_POLICY = "RetryPolicy";
        private const string CIRCUIT_BREAK_POLICY = "CircuitBreakerPolicy";

        public static void Configure(IServiceCollection services, IConfiguration configurations)
        {
            MaxRequisicoesParalelas = int.Parse(configurations.GetRequiredSection("Http")["MaxRequisicoesParalelas"]);

            var centroDistribuicaoUrl = configurations.GetRequiredSection("Http")["CentroDistribuicaoUrl"];

            var policyRegistry = services.AddHttpClient().AddPolicyRegistry();

            policyRegistry.Add(RETRY_POLICY, ResilienceServiceConfig.BuildRetryPolicy());
            policyRegistry.Add(CIRCUIT_BREAK_POLICY, ResilienceServiceConfig.BuildCircuitBreakPolicy());

            services
                .AddHttpClient(HTTP_CLIENT_CD, client =>
                {
                    client.BaseAddress = new Uri(centroDistribuicaoUrl!);
                    client.Timeout = TimeSpan.FromSeconds(10);
                })
                .AddPolicyHandlerFromRegistry(RETRY_POLICY)
                .AddPolicyHandlerFromRegistry(CIRCUIT_BREAK_POLICY); ;
        }
    }
}
