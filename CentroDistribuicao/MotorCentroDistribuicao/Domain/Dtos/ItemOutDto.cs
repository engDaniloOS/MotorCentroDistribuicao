namespace MotorCentroDistribuicao.Domain.Dtos
{
    public record ItemOutDto
    {
        public List<ItemDto> Itens { get; set; }

        public bool HasError { get; set; } = false;

        public string? ErrorMessage { get; set; }
    }
}