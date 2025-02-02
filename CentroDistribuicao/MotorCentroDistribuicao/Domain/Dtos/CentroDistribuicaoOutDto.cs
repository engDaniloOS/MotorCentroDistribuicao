namespace MotorCentroDistribuicao.Domain.Dtos
{
    public record CentroDistribuicaoOutDto
    {
        public List<CentroDistribuicaoDto> CentrosDistribuicao { get; set; }

        public bool HasError { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
