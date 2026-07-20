using System.Text.RegularExpressions;

namespace PortfolioApi.Validation;

/// <summary>
/// Valida o formato de tickers da B3: 4 letras seguidas de 1 ou 2 dígitos
/// (ex.: PETR4, TAEE11, MXRF11, BOVA11).
/// A validação é apenas de formato; não distingue o tipo do ativo
/// (Ação, FII ou ETF) a partir do sufixo numérico.
/// </summary>
public static partial class TickerValidator
{
    [GeneratedRegex(@"^[A-Z]{4}\d{1,2}$")]
    private static partial Regex TickerRegex();

    public static bool IsValid(string? ticker)
    {
        if (string.IsNullOrWhiteSpace(ticker))
            return false;

        return TickerRegex().IsMatch(ticker.Trim().ToUpperInvariant());
    }

    public static string Normalize(string ticker) => ticker.Trim().ToUpperInvariant();
}
