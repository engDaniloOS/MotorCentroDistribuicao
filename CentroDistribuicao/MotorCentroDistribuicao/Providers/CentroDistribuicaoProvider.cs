using MotorCentroDistribuicao.Configurations;
using MotorCentroDistribuicao.Domain.Providers;
using MotorCentroDistribuicao.Domain.Providers.Dtos;
using System.Text.Json;

namespace MotorCentroDistribuicao.Providers
{
    public class CentroDistribuicaoProvider(IHttpClientFactory httpClientFactory) : ICentroDistribuicaoProvider
    {

        private readonly HttpClient httpClient =
            httpClientFactory.CreateClient(HttpClientServiceConfig.HTTP_CLIENT_CD);

        public async Task<CentroDistribuicaoProviderDto> GetCentrosDistribuicaoPorItem(long item)
        {
            var url = $"/distribuitioncenters?itemId={item}";

            var response = await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<CentroDistribuicaoProviderDto>(json);
        }
    }
}
