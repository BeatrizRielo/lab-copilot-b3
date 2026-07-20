namespace PortfolioApi.Models;

/// <summary>
/// Representa um ativo da carteira do investidor (ação, FII ou ETF da B3).
/// </summary>
public class Ativo
{
    public int Id { get; set; }

    /// <summary>Ticker de negociação na B3 (ex.: PETR4, MXRF11, BOVA11).</summary>
    public string Ticker { get; set; } = string.Empty;

    /// <summary>Tipo do ativo: Acao, FII ou ETF.</summary>
    public TipoAtivo Tipo { get; set; }

    /// <summary>Quantidade total de cotas/ações em posição.</summary>
    public int Quantidade { get; set; }

    /// <summary>Preço médio de aquisição.</summary>
    public decimal PrecoMedio { get; set; }

    /// <summary>
    /// Token de concorrência otimista. Incrementado a cada alteração de posição
    /// para detectar atualizações concorrentes (ordens simultâneas no mesmo ativo).
    /// </summary>
    public int Version { get; set; }

    public ICollection<Ordem> Ordens { get; set; } = new List<Ordem>();
}

public enum TipoAtivo
{
    Acao,
    FII,
    ETF
}
