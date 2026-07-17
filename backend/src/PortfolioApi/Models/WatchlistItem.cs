namespace PortfolioApi.Models;

/// <summary>
/// Item da watchlist: um ticker monitorado com preço-alvo de interesse.
/// </summary>
public class WatchlistItem
{
    public int Id { get; set; }

    /// <summary>Ticker monitorado (ex.: VALE3).</summary>
    public string Ticker { get; set; } = string.Empty;

    /// <summary>Preço-alvo que o investidor deseja acompanhar.</summary>
    public decimal PrecoAlvo { get; set; }

    public DateTime CriadoEm { get; set; }
}
