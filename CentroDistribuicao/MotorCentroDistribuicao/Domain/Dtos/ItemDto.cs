using System.Text.Json.Serialization;

namespace MotorCentroDistribuicao.Domain.Dtos
{
    public record ItemDto
    {
        [JsonPropertyName("item")]
        public long Id { get; set; }

        [JsonPropertyName("erro")]
        public string Message { get; set; } = "";

        [JsonPropertyName("distribuitionCenters")]
        public List<string> CentrosDistribuicao { get; set; }
    }
}
