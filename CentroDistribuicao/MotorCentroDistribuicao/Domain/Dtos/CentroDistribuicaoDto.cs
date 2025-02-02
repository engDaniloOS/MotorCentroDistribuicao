using System.Text.Json.Serialization;

namespace MotorCentroDistribuicao.Domain.Dtos
{
    public class CentroDistribuicaoDto
    {
        [JsonPropertyName("distribuitionCenter")]
        public string Name { get; set; }

        [JsonPropertyName("itens")]
        public List<long> Itens { get; set; }
    }
}
