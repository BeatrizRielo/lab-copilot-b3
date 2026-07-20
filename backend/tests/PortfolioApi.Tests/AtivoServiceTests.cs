using PortfolioApi.Dtos;
using PortfolioApi.Models;
using PortfolioApi.Services;

namespace PortfolioApi.Tests;

public class AtivoServiceTests
{
    public static TheoryData<int, decimal> ValoresInvalidos => new()
    {
        { 0, 10m },
        { -1, 10m },
        { 10, 0m },
        { 10, -1m },
    };

    [Theory]
    [MemberData(nameof(ValoresInvalidos))]
    public async Task Ativo_ComQuantidadeOuPrecoInvalido_LancaExcecao(int quantidade, decimal precoMedio)
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;

        var service = new AtivoService(context);

        await Assert.ThrowsAsync<RegraNegocioException>(() =>
            service.CriarAsync(new AtivoCreateDto("PETR4", TipoAtivo.Acao, quantidade, precoMedio)));
    }

    [Fact]
    public async Task Ativo_Valido_CriaComSucesso()
    {
        var (context, conn) = TestDb.CreateContext();
        using var _ = conn;

        var service = new AtivoService(context);

        var criado = await service.CriarAsync(new AtivoCreateDto(" petr4 ", TipoAtivo.Acao, 10, 12.34m));

        Assert.Equal("PETR4", criado.Ticker);
        Assert.Equal(10, criado.Quantidade);
        Assert.Equal(12.34m, criado.PrecoMedio);
    }
}