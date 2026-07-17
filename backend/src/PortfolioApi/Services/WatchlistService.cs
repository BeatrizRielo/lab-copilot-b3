using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Dtos;
using PortfolioApi.Models;
using PortfolioApi.Validation;

namespace PortfolioApi.Services;

public class WatchlistService
{
    private readonly PortfolioContext _context;

    public WatchlistService(PortfolioContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<WatchlistDto>> ListarAsync()
    {
        return await _context.Watchlist
            .OrderBy(w => w.Ticker)
            .Select(w => new WatchlistDto(w.Id, w.Ticker, w.PrecoAlvo, w.CriadoEm))
            .ToListAsync();
    }

    public async Task<WatchlistDto?> ObterAsync(int id)
    {
        var item = await _context.Watchlist.FindAsync(id);
        return item is null ? null : Map(item);
    }

    public async Task<WatchlistDto> CriarAsync(WatchlistCreateDto dto)
    {
        ValidarEntrada(dto.Ticker, dto.PrecoAlvo);

        var item = new WatchlistItem
        {
            Ticker = TickerValidator.Normalize(dto.Ticker),
            PrecoAlvo = dto.PrecoAlvo,
            CriadoEm = DateTime.UtcNow
        };

        _context.Watchlist.Add(item);
        await _context.SaveChangesAsync();
        return Map(item);
    }

    public async Task<WatchlistDto> AtualizarAsync(int id, WatchlistUpdateDto dto)
    {
        var item = await _context.Watchlist.FindAsync(id)
            ?? throw new NaoEncontradoException($"Item {id} não encontrado na watchlist.");

        ValidarEntrada(dto.Ticker, dto.PrecoAlvo);

        item.Ticker = TickerValidator.Normalize(dto.Ticker);
        item.PrecoAlvo = dto.PrecoAlvo;

        await _context.SaveChangesAsync();
        return Map(item);
    }

    public async Task RemoverAsync(int id)
    {
        var item = await _context.Watchlist.FindAsync(id)
            ?? throw new NaoEncontradoException($"Item {id} não encontrado na watchlist.");

        _context.Watchlist.Remove(item);
        await _context.SaveChangesAsync();
    }

    private static void ValidarEntrada(string ticker, decimal precoAlvo)
    {
        if (!TickerValidator.IsValid(ticker))
            throw new RegraNegocioException($"Ticker inválido: '{ticker}'. Use o padrão da B3 (ex.: VALE3).");

        if (precoAlvo <= 0)
            throw new RegraNegocioException("Preço-alvo deve ser maior que zero.");
    }

    private static WatchlistDto Map(WatchlistItem w) => new(w.Id, w.Ticker, w.PrecoAlvo, w.CriadoEm);
}
