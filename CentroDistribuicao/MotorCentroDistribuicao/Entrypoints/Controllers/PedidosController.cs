using Microsoft.AspNetCore.Mvc;
using MotorCentroDistribuicao.Domain.Dtos;
using MotorCentroDistribuicao.Domain.UseCases;

namespace MotorCentroDistribuicao.Entrypoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidosController(
        IProcessarPedidoUseCase processarUseCase,
        IGetPedidoUseCase consultarUseCase) : ControllerBase
    {
        [HttpGet("/{pedidoId}")]
        public async Task<IActionResult> GetPedidoProcessado([FromRoute] Guid pedidoId)
        {
            var retorno = await consultarUseCase.GetPedidoProcessado(pedidoId);

            if (retorno.HasError)
                return BadRequest();

            return Ok(retorno);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessarItens([FromBody] PedidoDto pedido)
        {
            var retorno = await processarUseCase.GetCentrosDistribuicao(pedido);

            if (retorno.HasError)
                return BadRequest();

            return Ok(retorno);
        }

    }
}
