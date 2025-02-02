using System.Text.Json.Serialization;

namespace MotorCentroDistribuicao.Domain.Dtos
{
    public class ItemDto
    {
        [JsonPropertyName("item")]
        public long Id { get; set; }

        [JsonPropertyName("distribuitionCenters")]
        public List<string> CentrosDistribuicao { get; set; }
    }
}
