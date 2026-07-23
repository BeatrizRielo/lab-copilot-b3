using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Dtos;
using PortfolioApi.Services;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortfolioController(PortfolioService service) : ControllerBase
{
    /// <summary>Resumo consolidado da carteira com posição e resultado (P&L).</summary>
    [HttpGet("resumo")]
    public async Task<ActionResult<ResumoCarteiraDto>> Resumo()
        => Ok(await service.ObterResumoAsync());
}
