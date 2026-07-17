namespace PortfolioApi.Models;

/// <summary>
/// Representa uma ordem de compra ou venda de um ativo.
/// </summary>
public class Ordem
{
    public int Id { get; set; }

    public int AtivoId { get; set; }
    public Ativo? Ativo { get; set; }

    /// <summary>Tipo da operação: Compra ou Venda.</summary>
    public TipoOrdem Tipo { get; set; }

    public int Quantidade { get; set; }

    /// <summary>Preço unitário praticado na operação.</summary>
    public decimal Preco { get; set; }

    public DateTime Data { get; set; }
}

public enum TipoOrdem
{
    Compra,
    Venda
}
