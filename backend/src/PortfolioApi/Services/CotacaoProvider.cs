namespace PortfolioApi.Services;

/// <summary>
/// Fornece cotações simuladas para os ativos (mock didático).
/// Em produção seria integrado a um feed de mercado da B3.
/// </summary>
public interface ICotacaoProvider
{
    decimal ObterCotacao(string ticker);
}

public class CotacaoSimuladaProvider : ICotacaoProvider
{
    private static readonly Dictionary<string, decimal> Cotacoes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["PETR4"] = 38.00m,
        ["MXRF11"] = 10.50m,
        ["BOVA11"] = 122.00m,
        ["VALE3"] = 61.00m,
        ["ITUB4"] = 30.00m,
    };

    /// <summary>
    /// Retorna a cotação simulada do ticker. Se não houver cotação cadastrada,
    /// devolve o preço-base padrão para não quebrar o cálculo do resumo.
    /// </summary>
    public decimal ObterCotacao(string ticker)
    {
        return Cotacoes.TryGetValue(ticker, out var preco) ? preco : 20.00m;
    }
}
