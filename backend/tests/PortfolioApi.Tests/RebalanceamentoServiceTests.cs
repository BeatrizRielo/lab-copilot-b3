using Microsoft.Extensions.Options;
using PortfolioApi.Data;
using PortfolioApi.Dtos;
using PortfolioApi.Models;
using PortfolioApi.Services;

namespace PortfolioApi.Tests;

public class RebalanceamentoServiceTests
{
    private sealed class CotacaoDicionario : ICotacaoProvider
    {
        private readonly Dictionary<string, decimal> _cotacoes;
        public CotacaoDicionario(Dictionary<string, decimal> cotacoes) => _cotacoes = cotacoes;
        public decimal ObterCotacao(string ticker) => _cotacoes.TryGetValue(ticker, out var c) ? c : 0m;
    }

    private static RebalanceamentoOptions OpcoesPadrao(decimal corretagem = 0m) => new()
    {
        AlvosPorClasse = new() { ["Acao"] = 50m, ["FII"] = 30m, ["ETF"] = 20m },
        ToleranciaPercentual = 5m,
        CorretagemPorOrdem = corretagem,
        AliquotaIr = 0.15m,
    };

    private static RebalanceamentoService CriarService(
        PortfolioContext context,
        ICotacaoProvider cotacao,
        RebalanceamentoOptions? options = null)
        => new(context, cotacao, Options.Create(options ?? OpcoesPadrao()));

    // Carteira: Ação 60% (Acima), FII 30% (Equilibrada), ETF 10% (Abaixo). Total = 1000.
    private static void SemearCarteiraDesbalanceada(PortfolioContext context)
    {
        context.Ativos.Add(new Ativo { Ticker = "PETR4", Tipo = TipoAtivo.Acao, Quantidade = 10, PrecoMedio = 40m });
        context.Ativos.Add(new Ativo { Ticker = "MXRF11", Tipo = TipoAtivo.FII, Quantidade = 30, PrecoMedio = 10m });
        context.Ativos.Add(new Ativo { Ticker = "BOVA11", Tipo = TipoAtivo.ETF, Quantidade = 1, PrecoMedio = 100m });
        context.SaveChanges();
    }

    private static CotacaoDicionario CotacoesDesbalanceada() =>
        new(new() { ["PETR4"] = 60m, ["MXRF11"] = 10m, ["BOVA11"] = 100m });

    [Fact]
    public async Task Percentual_PorClasse_CalculadoSobreValorTotal()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;
        SemearCarteiraDesbalanceada(context);

        var service = CriarService(context, CotacoesDesbalanceada());
        var resultado = await service.ObterSugestaoAsync();

        var acao = resultado.Alocacoes.Single(a => a.Classe == "Acao");
        Assert.Equal(60m, acao.PercentualAtual);
        Assert.Equal(50m, acao.PercentualAlvo);

        var fii = resultado.Alocacoes.Single(a => a.Classe == "FII");
        Assert.Equal(30m, fii.PercentualAtual);

        var etf = resultado.Alocacoes.Single(a => a.Classe == "ETF");
        Assert.Equal(10m, etf.PercentualAtual);
    }

    [Fact]
    public async Task Situacao_SinalizaConcentracaoExcessivaEInsuficiente()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;
        SemearCarteiraDesbalanceada(context);

        var service = CriarService(context, CotacoesDesbalanceada());
        var resultado = await service.ObterSugestaoAsync();

        Assert.Equal(SituacaoClasse.Acima, resultado.Alocacoes.Single(a => a.Classe == "Acao").Situacao);
        Assert.Equal(SituacaoClasse.Equilibrada, resultado.Alocacoes.Single(a => a.Classe == "FII").Situacao);
        Assert.Equal(SituacaoClasse.Abaixo, resultado.Alocacoes.Single(a => a.Classe == "ETF").Situacao);
    }

    [Fact]
    public async Task Sugestoes_ReduzemClasseAcima_EAumentamClasseAbaixo()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;
        SemearCarteiraDesbalanceada(context);

        var service = CriarService(context, CotacoesDesbalanceada());
        var resultado = await service.ObterSugestaoAsync();

        Assert.Contains(resultado.Sugestoes, s => s.Ticker == "PETR4" && s.Acao == AcaoRebalanceamento.Reduzir);
        Assert.Contains(resultado.Sugestoes, s => s.Ticker == "BOVA11" && s.Acao == AcaoRebalanceamento.Aumentar);
    }

    [Fact]
    public async Task Custo_IncluiIr15PorcentoSobreLucroDaVenda()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;
        SemearCarteiraDesbalanceada(context);

        // PETR4: reduzir 1 (floor(100/60)); lucro = (60-40)*1 = 20; IR = 15% * 20 = 3.
        var service = CriarService(context, CotacoesDesbalanceada());
        var resultado = await service.ObterSugestaoAsync();

        Assert.Equal(3m, resultado.Custo.IrEstimado);
        Assert.Equal(0m, resultado.Custo.CorretagemTotal);
        Assert.Equal(3m, resultado.Custo.CustoTotal);
    }

    [Fact]
    public async Task Custo_SomaCorretagemPorOrdemSugerida()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;
        SemearCarteiraDesbalanceada(context);

        // 2 ordens (reduzir PETR4 + aumentar BOVA11) * 4.90 = 9.80; IR = 3; total = 12.80.
        var service = CriarService(context, CotacoesDesbalanceada(), OpcoesPadrao(corretagem: 4.90m));
        var resultado = await service.ObterSugestaoAsync();

        Assert.Equal(9.80m, resultado.Custo.CorretagemTotal);
        Assert.Equal(3m, resultado.Custo.IrEstimado);
        Assert.Equal(12.80m, resultado.Custo.CustoTotal);
    }

    [Fact]
    public async Task Custo_IrZero_QuandoVendaNaoTemLucro()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;
        // Ação Acima, mas preço médio igual à cotação → lucro 0 → IR 0.
        context.Ativos.Add(new Ativo { Ticker = "PETR4", Tipo = TipoAtivo.Acao, Quantidade = 10, PrecoMedio = 60m });
        context.Ativos.Add(new Ativo { Ticker = "MXRF11", Tipo = TipoAtivo.FII, Quantidade = 30, PrecoMedio = 10m });
        context.Ativos.Add(new Ativo { Ticker = "BOVA11", Tipo = TipoAtivo.ETF, Quantidade = 1, PrecoMedio = 100m });
        context.SaveChanges();

        var service = CriarService(context, CotacoesDesbalanceada());
        var resultado = await service.ObterSugestaoAsync();

        Assert.Contains(resultado.Sugestoes, s => s.Ticker == "PETR4" && s.Acao == AcaoRebalanceamento.Reduzir);
        Assert.Equal(0m, resultado.Custo.IrEstimado);
    }

    [Fact]
    public async Task CarteiraVazia_NaoGeraSugestoes_ECustoZero()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;

        var service = CriarService(context, new CotacaoDicionario(new()));
        var resultado = await service.ObterSugestaoAsync();

        Assert.Empty(resultado.Sugestoes);
        Assert.Equal(0m, resultado.Custo.CustoTotal);
        Assert.All(resultado.Alocacoes, a => Assert.Equal(0m, a.PercentualAtual));
    }

    [Fact]
    public async Task CarteiraEquilibrada_NaoGeraSugestoes()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;
        // Ação 50% / FII 30% / ETF 20%, total 1000.
        context.Ativos.Add(new Ativo { Ticker = "PETR4", Tipo = TipoAtivo.Acao, Quantidade = 10, PrecoMedio = 40m });
        context.Ativos.Add(new Ativo { Ticker = "MXRF11", Tipo = TipoAtivo.FII, Quantidade = 30, PrecoMedio = 10m });
        context.Ativos.Add(new Ativo { Ticker = "BOVA11", Tipo = TipoAtivo.ETF, Quantidade = 2, PrecoMedio = 100m });
        context.SaveChanges();

        var cot = new CotacaoDicionario(new() { ["PETR4"] = 50m, ["MXRF11"] = 10m, ["BOVA11"] = 100m });
        var service = CriarService(context, cot);
        var resultado = await service.ObterSugestaoAsync();

        Assert.All(resultado.Alocacoes, a => Assert.Equal(SituacaoClasse.Equilibrada, a.Situacao));
        Assert.Empty(resultado.Sugestoes);
    }

    [Fact]
    public async Task Sugestao_NuncaVendeAcimaDaPosicao()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;
        SemearCarteiraDesbalanceada(context);

        var service = CriarService(context, CotacoesDesbalanceada());
        var resultado = await service.ObterSugestaoAsync();

        foreach (var sugestao in resultado.Sugestoes.Where(s => s.Acao == AcaoRebalanceamento.Reduzir))
        {
            var ativo = context.Ativos.Single(a => a.Ticker == sugestao.Ticker);
            Assert.True(sugestao.Quantidade <= ativo.Quantidade);
        }
    }
}
