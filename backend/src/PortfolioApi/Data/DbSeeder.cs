using PortfolioApi.Models;

namespace PortfolioApi.Data;

/// <summary>
/// Popula o banco em memória com dados de exemplo da B3 para o workshop.
/// </summary>
public static class DbSeeder
{
    public static void Seed(PortfolioContext context)
    {
        if (context.Ativos.Any())
            return;

        var petr4 = new Ativo { Ticker = "PETR4", Tipo = TipoAtivo.Acao, Quantidade = 100, PrecoMedio = 32.50m };
        var mxrf11 = new Ativo { Ticker = "MXRF11", Tipo = TipoAtivo.FII, Quantidade = 200, PrecoMedio = 10.20m };
        var bova11 = new Ativo { Ticker = "BOVA11", Tipo = TipoAtivo.ETF, Quantidade = 50, PrecoMedio = 110.00m };

        context.Ativos.AddRange(petr4, mxrf11, bova11);

        context.Ordens.AddRange(
            new Ordem { Ativo = petr4, Tipo = TipoOrdem.Compra, Quantidade = 100, Preco = 32.50m, Data = DateTime.UtcNow.AddDays(-30) },
            new Ordem { Ativo = mxrf11, Tipo = TipoOrdem.Compra, Quantidade = 200, Preco = 10.20m, Data = DateTime.UtcNow.AddDays(-20) },
            new Ordem { Ativo = bova11, Tipo = TipoOrdem.Compra, Quantidade = 50, Preco = 110.00m, Data = DateTime.UtcNow.AddDays(-10) }
        );

        context.Watchlist.AddRange(
            new WatchlistItem { Ticker = "VALE3", PrecoAlvo = 60.00m, CriadoEm = DateTime.UtcNow },
            new WatchlistItem { Ticker = "ITUB4", PrecoAlvo = 28.00m, CriadoEm = DateTime.UtcNow }
        );

        context.SaveChanges();
    }
}
