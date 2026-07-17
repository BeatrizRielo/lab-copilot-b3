using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Dtos;

namespace PortfolioApi.Services;

/// <summary>
/// Calcula o resumo consolidado da carteira (posição, valor atual e P&L).
/// </summary>
public class PortfolioService
{
    private readonly PortfolioContext _context;
    private readonly ICotacaoProvider _cotacaoProvider;

    public PortfolioService(PortfolioContext context, ICotacaoProvider cotacaoProvider)
    {
        _context = context;
        _cotacaoProvider = cotacaoProvider;
    }

    public async Task<ResumoCarteiraDto> ObterResumoAsync()
    {
        var ativos = await _context.Ativos.AsNoTracking().ToListAsync();

        var posicoes = new List<ResumoPosicaoDto>();
        decimal totalInvestido = 0m;
        decimal totalAtual = 0m;

        foreach (var ativo in ativos)
        {
            var precoAtual = _cotacaoProvider.ObterCotacao(ativo.Ticker);
            var valorInvestido = ativo.PrecoMedio * ativo.Quantidade;
            var valorAtual = precoAtual * ativo.Quantidade;
            var resultado = valorAtual - valorInvestido;
            var resultadoPercentual = valorInvestido == 0
                ? 0m
                : Math.Round(resultado / valorInvestido * 100m, 2);

            posicoes.Add(new ResumoPosicaoDto(
                ativo.Ticker,
                ativo.Quantidade,
                ativo.PrecoMedio,
                precoAtual,
                valorInvestido,
                valorAtual,
                resultado,
                resultadoPercentual));

            totalInvestido += valorInvestido;
            totalAtual += valorAtual;
        }

        var resultadoTotal = totalAtual - totalInvestido;
        var resultadoPercentualTotal = totalInvestido == 0
            ? 0m
            : Math.Round(resultadoTotal / totalInvestido * 100m, 2);

        return new ResumoCarteiraDto(
            totalInvestido,
            totalAtual,
            resultadoTotal,
            resultadoPercentualTotal,
            posicoes);
    }
}
