using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Dtos;
using PortfolioApi.Services;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdensController : ControllerBase
{
    private readonly OrdemService _service;

    public OrdensController(OrdemService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrdemDto>>> Listar()
        => Ok(await _service.ListarAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrdemDto>> Obter(int id)
    {
        var ordem = await _service.ObterAsync(id);
        return ordem is null ? NotFound() : Ok(ordem);
    }

    [HttpPost]
    public async Task<ActionResult<OrdemDto>> Criar(OrdemCreateDto dto)
    {
        var criada = await _service.CriarAsync(dto);
        return CreatedAtAction(nameof(Obter), new { id = criada.Id }, criada);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id)
    {
        await _service.RemoverAsync(id);
        return NoContent();
    }
}
