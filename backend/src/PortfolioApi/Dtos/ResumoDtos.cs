namespace PortfolioApi.Dtos;

/// <summary>
/// Linha do resumo consolidado por ativo.
/// </summary>
public record ResumoPosicaoDto(
    string Ticker,
    int Quantidade,
    decimal PrecoMedio,
    decimal PrecoAtual,
    decimal ValorInvestido,
    decimal ValorAtual,
    decimal Resultado,
    decimal ResultadoPercentual);

/// <summary>
/// Resumo consolidado da carteira inteira.
/// </summary>
public record ResumoCarteiraDto(
    decimal TotalInvestido,
    decimal TotalAtual,
    decimal ResultadoTotal,
    decimal ResultadoPercentualTotal,
    IReadOnlyList<ResumoPosicaoDto> Posicoes);
