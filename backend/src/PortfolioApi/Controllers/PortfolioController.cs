using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Dtos;
using PortfolioApi.Services;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortfolioController : ControllerBase
{
    private readonly PortfolioService _service;

    public PortfolioController(PortfolioService service)
    {
        _service = service;
    }

    /// <summary>Resumo consolidado da carteira com posição e resultado (P&L).</summary>
    [HttpGet("resumo")]
    public async Task<ActionResult<ResumoCarteiraDto>> Resumo()
        => Ok(await _service.ObterResumoAsync());
}
