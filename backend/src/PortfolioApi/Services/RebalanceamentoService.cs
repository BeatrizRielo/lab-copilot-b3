using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PortfolioApi.Data;
using PortfolioApi.Dtos;
using PortfolioApi.Models;

namespace PortfolioApi.Services;

/// <summary>
/// Analisa a carteira, identifica desvios frente aos alvos por classe e
/// sugere ajustes (compra/venda) com estimativa de custo (corretagem + IR).
/// </summary>
public class RebalanceamentoService
{
    private readonly PortfolioContext _context;
    private readonly ICotacaoProvider _cotacaoProvider;
    private readonly RebalanceamentoOptions _options;

    public RebalanceamentoService(
        PortfolioContext context,
        ICotacaoProvider cotacaoProvider,
        IOptions<RebalanceamentoOptions> options)
    {
        _context = context;
        _cotacaoProvider = cotacaoProvider;
        _options = options.Value;
    }

    /// <summary>
    /// Calcula a alocação atual por classe, gera sugestões de ajuste priorizando
    /// os maiores desvios e estima o custo total das ordens sugeridas.
    /// </summary>
    public async Task<RebalanceamentoDto> ObterSugestaoAsync()
    {
        var ativos = await _context.Ativos.AsNoTracking().ToListAsync();

        var posicoes = ativos
            .Select(a =>
            {
                var cotacao = _cotacaoProvider.ObterCotacao(a.Ticker);
                return new Posicao(a, cotacao, cotacao * a.Quantidade);
            })
            .ToList();

        var totalAtual = posicoes.Sum(p => p.ValorAtual);

        var alocacoes = new List<AlocacaoClasseDto>();
        var sugestoes = new List<SugestaoAjusteDto>();
        decimal corretagemTotal = 0m;
        decimal lucroTributavel = 0m;

        var tolerancia = _options.ToleranciaPercentual;

        // 1ª passada: analisa cada classe (percentual atual, situação e valores).
        // A lista de alocações segue a ordem configurada em AlvosPorClasse.
        var analises = new List<AnaliseClasse>();
        foreach (var (classeNome, alvo) in _options.AlvosPorClasse)
        {
            if (!Enum.TryParse<TipoAtivo>(classeNome, out var tipo))
                continue; // guard defensivo; config já validada no startup.

            var doClasse = posicoes.Where(p => p.Ativo.Tipo == tipo).ToList();
            var valorClasse = doClasse.Sum(p => p.ValorAtual);
            var percentualAtual = totalAtual == 0m
                ? 0m
                : Math.Round(valorClasse / totalAtual * 100m, 2);

            var situacao = percentualAtual > alvo + tolerancia ? SituacaoClasse.Acima
                : percentualAtual < alvo - tolerancia ? SituacaoClasse.Abaixo
                : SituacaoClasse.Equilibrada;

            alocacoes.Add(new AlocacaoClasseDto(classeNome, percentualAtual, alvo, situacao));
            analises.Add(new AnaliseClasse(alvo, percentualAtual, situacao, valorClasse, doClasse));
        }

        // Carteira vazia / valor total zero: sem sugestões e custo zero (sem divisão por zero).
        if (totalAtual == 0m)
        {
            var custoVazio = new CustoRebalanceamentoDto(0m, 0m, 0m);
            return new RebalanceamentoDto(alocacoes, sugestoes, custoVazio);
        }

        // 2ª passada: RN-006 / RF-008 — gera as sugestões priorizando as classes
        // com o maior desvio (em pontos percentuais) em relação ao alvo.
        var classesPorDesvio = analises
            .OrderByDescending(a => Math.Abs(a.PercentualAtual - a.Alvo));

        foreach (var analise in classesPorDesvio)
        {
            var valorAlvo = analise.Alvo / 100m * totalAtual;

            if (analise.Situacao == SituacaoClasse.Acima)
            {
                // Reduz o excesso começando pelo ativo de maior valor na classe.
                var valorReduzir = analise.ValorClasse - valorAlvo;
                foreach (var p in analise.Posicoes.OrderByDescending(x => x.ValorAtual))
                {
                    if (valorReduzir <= 0m || p.Cotacao <= 0m)
                        continue;

                    // RN-007: nunca vender acima da posição atual.
                    var qtd = Math.Min(
                        (int)Math.Floor(valorReduzir / p.Cotacao),
                        p.Ativo.Quantidade);
                    if (qtd <= 0)
                        continue;

                    sugestoes.Add(new SugestaoAjusteDto(p.Ativo.Ticker, AcaoRebalanceamento.Reduzir, qtd));
                    corretagemTotal += _options.CorretagemPorOrdem;

                    // RN-005: IR incide apenas sobre lucro positivo da venda.
                    // Acumula o lucro bruto e arredonda o IR uma única vez no fim,
                    // evitando acúmulo de erro de arredondamento por ordem.
                    var lucro = (p.Cotacao - p.Ativo.PrecoMedio) * qtd;
                    if (lucro > 0m)
                        lucroTributavel += lucro;

                    valorReduzir -= qtd * p.Cotacao;
                }
            }
            else if (analise.Situacao == SituacaoClasse.Abaixo)
            {
                // Aumenta a alocação reforçando o ativo de maior valor na classe.
                // Requer ao menos um ativo já existente na classe para gerar a ordem.
                var valorAumentar = valorAlvo - analise.ValorClasse;
                var alvoAtivo = analise.Posicoes.OrderByDescending(x => x.ValorAtual).FirstOrDefault();
                if (alvoAtivo is not null && alvoAtivo.Cotacao > 0m)
                {
                    var qtd = (int)Math.Floor(valorAumentar / alvoAtivo.Cotacao);
                    if (qtd > 0)
                    {
                        sugestoes.Add(new SugestaoAjusteDto(alvoAtivo.Ativo.Ticker, AcaoRebalanceamento.Aumentar, qtd));
                        corretagemTotal += _options.CorretagemPorOrdem;
                    }
                }
            }
        }

        var irEstimado = Math.Round(lucroTributavel * _options.AliquotaIr, 2);
        var custo = new CustoRebalanceamentoDto(corretagemTotal, irEstimado, corretagemTotal + irEstimado);
        return new RebalanceamentoDto(alocacoes, sugestoes, custo);
    }

    private sealed record Posicao(Ativo Ativo, decimal Cotacao, decimal ValorAtual);

    /// <summary>Resultado da análise de uma classe, usado para ordenar as sugestões por desvio.</summary>
    private sealed record AnaliseClasse(
        decimal Alvo,
        decimal PercentualAtual,
        SituacaoClasse Situacao,
        decimal ValorClasse,
        IReadOnlyList<Posicao> Posicoes);
}
