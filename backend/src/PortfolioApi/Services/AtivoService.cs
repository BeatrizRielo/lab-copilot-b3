using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Dtos;
using PortfolioApi.Models;
using PortfolioApi.Validation;

namespace PortfolioApi.Services;

public class AtivoService
{
    private readonly PortfolioContext _context;

    public AtivoService(PortfolioContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<AtivoDto>> ListarAsync()
    {
        return await _context.Ativos
            .OrderBy(a => a.Ticker)
            .Select(a => new AtivoDto(a.Id, a.Ticker, a.Tipo, a.Quantidade, a.PrecoMedio))
            .ToListAsync();
    }

    public async Task<AtivoDto?> ObterAsync(int id)
    {
        var ativo = await _context.Ativos.FindAsync(id);
        return ativo is null ? null : Map(ativo);
    }

    public async Task<AtivoDto> CriarAsync(AtivoCreateDto dto)
    {
        ValidarEntrada(dto.Ticker, dto.Quantidade, dto.PrecoMedio);

        var ticker = TickerValidator.Normalize(dto.Ticker);

        if (await _context.Ativos.AnyAsync(a => a.Ticker == ticker))
            throw new RegraNegocioException($"Já existe um ativo com o ticker {ticker}.");

        var ativo = new Ativo
        {
            Ticker = ticker,
            Tipo = dto.Tipo,
            Quantidade = dto.Quantidade,
            PrecoMedio = dto.PrecoMedio
        };

        _context.Ativos.Add(ativo);
        await _context.SaveChangesAsync();
        return Map(ativo);
    }

    public async Task<AtivoDto> AtualizarAsync(int id, AtivoUpdateDto dto)
    {
        var ativo = await _context.Ativos.FindAsync(id)
            ?? throw new NaoEncontradoException($"Ativo {id} não encontrado.");

        ValidarEntrada(dto.Ticker, dto.Quantidade, dto.PrecoMedio);

        ativo.Ticker = TickerValidator.Normalize(dto.Ticker);
        ativo.Tipo = dto.Tipo;
        ativo.Quantidade = dto.Quantidade;
        ativo.PrecoMedio = dto.PrecoMedio;

        await _context.SaveChangesAsync();
        return Map(ativo);
    }

    public async Task RemoverAsync(int id)
    {
        var ativo = await _context.Ativos.FindAsync(id)
            ?? throw new NaoEncontradoException($"Ativo {id} não encontrado.");

        _context.Ativos.Remove(ativo);
        await _context.SaveChangesAsync();
    }

    private static void ValidarEntrada(string ticker, int quantidade, decimal precoMedio)
    {
        if (!TickerValidator.IsValid(ticker))
            throw new RegraNegocioException($"Ticker inválido: '{ticker}'. Use o padrão da B3 (ex.: PETR4, MXRF11).");

        if (quantidade < 0)
            throw new RegraNegocioException("Quantidade não pode ser negativa.");

        if (precoMedio < 0)
            throw new RegraNegocioException("Preço médio não pode ser negativo.");
    }

    private static AtivoDto Map(Ativo a) => new(a.Id, a.Ticker, a.Tipo, a.Quantidade, a.PrecoMedio);
}
