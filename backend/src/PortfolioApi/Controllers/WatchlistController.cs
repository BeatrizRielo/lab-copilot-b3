using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Dtos;
using PortfolioApi.Services;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WatchlistController : ControllerBase
{
    private readonly WatchlistService _service;

    public WatchlistController(WatchlistService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<WatchlistDto>>> Listar()
        => Ok(await _service.ListarAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<WatchlistDto>> Obter(int id)
    {
        var item = await _service.ObterAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<WatchlistDto>> Criar(WatchlistCreateDto dto)
    {
        var criado = await _service.CriarAsync(dto);
        return CreatedAtAction(nameof(Obter), new { id = criado.Id }, criado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<WatchlistDto>> Atualizar(int id, WatchlistUpdateDto dto)
        => Ok(await _service.AtualizarAsync(id, dto));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Remover(int id)
    {
        await _service.RemoverAsync(id);
        return NoContent();
    }
}
