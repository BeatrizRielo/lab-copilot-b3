using PortfolioApi.Models;
using PortfolioApi.Services;

namespace PortfolioApi.Tests;

public class PortfolioServiceTests
{
    private sealed class CotacaoFixa : ICotacaoProvider
    {
        private readonly decimal _preco;
        public CotacaoFixa(decimal preco) => _preco = preco;
        public decimal ObterCotacao(string ticker) => _preco;
    }

    [Fact]
    public async Task Resumo_CalculaResultadoPositivo()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;

        // 100 ações compradas a 30 (investido = 3000), cotação atual 40 (atual = 4000)
        context.Ativos.Add(new Ativo { Ticker = "PETR4", Tipo = TipoAtivo.Acao, Quantidade = 100, PrecoMedio = 30m });
        await context.SaveChangesAsync();

        var service = new PortfolioService(context, new CotacaoFixa(40m));
        var resumo = await service.ObterResumoAsync();

        Assert.Equal(3000m, resumo.TotalInvestido);
        Assert.Equal(4000m, resumo.TotalAtual);
        Assert.Equal(1000m, resumo.ResultadoTotal);
        Assert.Equal(33.33m, resumo.ResultadoPercentualTotal);
    }

    [Fact]
    public async Task Resumo_CalculaResultadoNegativo()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;

        context.Ativos.Add(new Ativo { Ticker = "PETR4", Tipo = TipoAtivo.Acao, Quantidade = 100, PrecoMedio = 50m });
        await context.SaveChangesAsync();

        var service = new PortfolioService(context, new CotacaoFixa(40m));
        var resumo = await service.ObterResumoAsync();

        Assert.Equal(-1000m, resumo.ResultadoTotal);
        Assert.Single(resumo.Posicoes);
    }

    [Fact]
    public async Task Resumo_CarteiraVazia_RetornaZeros()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;

        var service = new PortfolioService(context, new CotacaoFixa(40m));
        var resumo = await service.ObterResumoAsync();

        Assert.Equal(0m, resumo.TotalInvestido);
        Assert.Equal(0m, resumo.ResultadoPercentualTotal);
        Assert.Empty(resumo.Posicoes);
    }
}
