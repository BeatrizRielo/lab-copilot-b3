using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Dtos;
using PortfolioApi.Services;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RebalanceamentoController(RebalanceamentoService rebalanceamentoService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<RebalanceamentoDto>> ObterSugestao()
    {
        var r = await rebalanceamentoService.ObterSugestaoAsync();

        if (r.Sugestoes.Count > 50)
            Response.Headers.Append("X-Rebalance-Warning", "muitas-sugestoes");

        return Ok(r);
    }
}
