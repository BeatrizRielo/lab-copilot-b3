using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Dtos;
using PortfolioApi.Models;

namespace PortfolioApi.Services;

public class OrdemService
{
    private readonly PortfolioContext _context;

    public OrdemService(PortfolioContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<OrdemDto>> ListarAsync()
    {
        return await _context.Ordens
            .Include(o => o.Ativo)
            .OrderByDescending(o => o.Data)
            .Select(o => new OrdemDto(o.Id, o.AtivoId, o.Ativo!.Ticker, o.Tipo, o.Quantidade, o.Preco, o.Data))
            .ToListAsync();
    }

    public async Task<OrdemDto?> ObterAsync(int id)
    {
        var ordem = await _context.Ordens.Include(o => o.Ativo).FirstOrDefaultAsync(o => o.Id == id);
        return ordem is null ? null : Map(ordem);
    }

    public async Task<OrdemDto> CriarAsync(OrdemCreateDto dto)
    {
        var ativo = await _context.Ativos.FindAsync(dto.AtivoId)
            ?? throw new RegraNegocioException($"Ativo {dto.AtivoId} não encontrado.");

        ValidarEntrada(dto.Quantidade, dto.Preco);

        if (dto.Tipo == TipoOrdem.Venda && dto.Quantidade > ativo.Quantidade)
            throw new RegraNegocioException(
                $"Venda de {dto.Quantidade} excede a posição atual de {ativo.Quantidade} em {ativo.Ticker}.");

        var ordem = new Ordem
        {
            AtivoId = dto.AtivoId,
            Tipo = dto.Tipo,
            Quantidade = dto.Quantidade,
            Preco = dto.Preco,
            Data = dto.Data ?? DateTime.UtcNow
        };

        AtualizarPosicao(ativo, ordem);

        _context.Ordens.Add(ordem);
        await _context.SaveChangesAsync();

        ordem.Ativo = ativo;
        return Map(ordem);
    }

    public async Task RemoverAsync(int id)
    {
        var ordem = await _context.Ordens.FindAsync(id)
            ?? throw new NaoEncontradoException($"Ordem {id} não encontrada.");

        _context.Ordens.Remove(ordem);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Atualiza quantidade e preço médio do ativo ao registrar uma ordem.
    /// Compra: recalcula preço médio ponderado. Venda: reduz a quantidade
    /// mantendo o preço médio (padrão contábil para apuração de resultado).
    /// </summary>
    private static void AtualizarPosicao(Ativo ativo, Ordem ordem)
    {
        if (ordem.Tipo == TipoOrdem.Compra)
        {
            var custoAtual = ativo.PrecoMedio * ativo.Quantidade;
            var custoNovo = ordem.Preco * ordem.Quantidade;
            var novaQuantidade = ativo.Quantidade + ordem.Quantidade;

            ativo.PrecoMedio = novaQuantidade == 0 ? 0 : (custoAtual + custoNovo) / novaQuantidade;
            ativo.Quantidade = novaQuantidade;
        }
        else
        {
            ativo.Quantidade -= ordem.Quantidade;
        }
    }

    private static void ValidarEntrada(int quantidade, decimal preco)
    {
        if (quantidade <= 0)
            throw new RegraNegocioException("Quantidade da ordem deve ser maior que zero.");

        if (preco <= 0)
            throw new RegraNegocioException("Preço da ordem deve ser maior que zero.");
    }

    private static OrdemDto Map(Ordem o) =>
        new(o.Id, o.AtivoId, o.Ativo!.Ticker, o.Tipo, o.Quantidade, o.Preco, o.Data);
}
