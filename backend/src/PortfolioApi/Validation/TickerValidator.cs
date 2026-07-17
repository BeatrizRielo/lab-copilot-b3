using System.Text.RegularExpressions;

namespace PortfolioApi.Validation;

/// <summary>
/// Valida tickers no padrão da B3.
/// Ações: 4 letras + 1 ou 2 dígitos (ex.: PETR4, TAEE11).
/// FIIs/ETFs: 4 letras + 11 (ex.: MXRF11, BOVA11).
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
