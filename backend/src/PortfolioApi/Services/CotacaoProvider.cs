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
    /// <summary>
    /// Preço-base usado quando o ticker não possui cotação simulada cadastrada.
    /// Mantido explícito para deixar claro que é um valor de fallback do mock,
    /// e não uma cotação real de mercado.
    /// </summary>
    public const decimal CotacaoPadrao = 20.00m;

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
    /// devolve <see cref="CotacaoPadrao"/> para não quebrar o cálculo do resumo.
    /// Numa integração real, um ticker sem cotação deveria lançar exceção ou
    /// retornar um valor opcional (null) tratado pelo chamador.
    /// </summary>
    public decimal ObterCotacao(string ticker)
    {
        return Cotacoes.TryGetValue(ticker, out var preco) ? preco : CotacaoPadrao;
    }
}
