using System.Text.Json.Serialization;

namespace MotorCentroDistribuicao.Domain.Dtos
{
    public record PedidoOutDto
    {
        [JsonPropertyName("pedidoId")]
        public Guid Id { get; set; }

        [JsonPropertyName("itens")]
        public List<ItemDto> Itens { get; set; }

        [JsonPropertyName("validade")]
        public DateTime Validade { get; set; }

        [JsonIgnore]
        public bool HasError { get; set; } = false;

        [JsonPropertyName("erro")]
        public string? ErrorMessage { get; set; }
    }
}