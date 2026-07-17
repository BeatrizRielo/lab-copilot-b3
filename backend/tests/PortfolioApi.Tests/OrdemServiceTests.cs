using PortfolioApi.Dtos;
using PortfolioApi.Models;
using PortfolioApi.Services;

namespace PortfolioApi.Tests;

public class OrdemServiceTests
{
    [Fact]
    public async Task Compra_RecalculaPrecoMedioPonderado()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;

        var ativo = new Ativo { Ticker = "PETR4", Tipo = TipoAtivo.Acao, Quantidade = 100, PrecoMedio = 30m };
        context.Ativos.Add(ativo);
        await context.SaveChangesAsync();

        var service = new OrdemService(context);

        // Compra 100 a 40 => preço médio deve ir para (100*30 + 100*40)/200 = 35
        await service.CriarAsync(new OrdemCreateDto(ativo.Id, TipoOrdem.Compra, 100, 40m, null));

        Assert.Equal(200, ativo.Quantidade);
        Assert.Equal(35m, ativo.PrecoMedio);
    }

    [Fact]
    public async Task Venda_ReduzQuantidadeMantemPrecoMedio()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;

        var ativo = new Ativo { Ticker = "PETR4", Tipo = TipoAtivo.Acao, Quantidade = 100, PrecoMedio = 30m };
        context.Ativos.Add(ativo);
        await context.SaveChangesAsync();

        var service = new OrdemService(context);
        await service.CriarAsync(new OrdemCreateDto(ativo.Id, TipoOrdem.Venda, 40, 45m, null));

        Assert.Equal(60, ativo.Quantidade);
        Assert.Equal(30m, ativo.PrecoMedio);
    }

    [Fact]
    public async Task Venda_MaiorQuePosicao_LancaExcecao()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;

        var ativo = new Ativo { Ticker = "PETR4", Tipo = TipoAtivo.Acao, Quantidade = 50, PrecoMedio = 30m };
        context.Ativos.Add(ativo);
        await context.SaveChangesAsync();

        var service = new OrdemService(context);

        await Assert.ThrowsAsync<RegraNegocioException>(() =>
            service.CriarAsync(new OrdemCreateDto(ativo.Id, TipoOrdem.Venda, 100, 45m, null)));
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(-5, 10)]
    [InlineData(10, 0)]
    [InlineData(10, -1)]
    public async Task Ordem_ComValoresInvalidos_LancaExcecao(int quantidade, decimal preco)
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;

        var ativo = new Ativo { Ticker = "PETR4", Tipo = TipoAtivo.Acao, Quantidade = 100, PrecoMedio = 30m };
        context.Ativos.Add(ativo);
        await context.SaveChangesAsync();

        var service = new OrdemService(context);

        await Assert.ThrowsAsync<RegraNegocioException>(() =>
            service.CriarAsync(new OrdemCreateDto(ativo.Id, TipoOrdem.Compra, quantidade, preco, null)));
    }
}
