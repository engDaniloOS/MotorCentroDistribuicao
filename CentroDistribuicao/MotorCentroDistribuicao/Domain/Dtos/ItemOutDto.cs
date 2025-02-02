using System.Text.Json.Serialization;

namespace MotorCentroDistribuicao.Domain.Dtos
{
    public record ItemOutDto
    {
        [JsonPropertyName("pedidoId")]
        public Guid Id { get; } = Guid.NewGuid();

        [JsonPropertyName("itens")]
        public List<ItemDto> Itens { get; set; }

        [JsonIgnore]
        public bool HasError { get; set; } = false;

        [JsonIgnore]
        public string? ErrorMessage { get; set; }
    }
}