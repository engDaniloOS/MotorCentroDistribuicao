using Microsoft.AspNetCore.Mvc;
using MotorCentroDistribuicao.Domain.Dtos;
using MotorCentroDistribuicao.Domain.Services;

namespace MotorCentroDistribuicao.Entrypoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidosController(IPedidosService service) : ControllerBase
    {
        [HttpPost("distribuitionsCenters")]
        public async Task<IActionResult> ProcessarCentrosDistribuicao([FromBody] PedidoDto pedido)
        {
            var dto = await service.GetCentrosDistribuicaoComItensVinculados(pedido);

            if (dto.HasError)
                return BadRequest();

            return Ok(dto.CentrosDistribuicao);
        }

        [HttpPost("itens")]
        public async Task<IActionResult> ProcessarItens([FromBody] PedidoDto pedido)
        {
            var dto = await service.GetItensComCentrosDistribuicaoVinculados(pedido);

            if (dto.HasError)
                return BadRequest();

            return Ok(dto.Itens);
        }

    }
}
