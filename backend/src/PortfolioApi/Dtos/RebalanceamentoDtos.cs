namespace PortfolioApi.Dtos;

/// <summary>
/// Situação de uma classe de ativos frente à alocação-alvo configurada.
/// </summary>
public enum SituacaoClasse
{
    /// <summary>Percentual atual abaixo da faixa-alvo (concentração insuficiente).</summary>
    Abaixo,

    /// <summary>Percentual atual dentro da faixa-alvo (alvo ± tolerância).</summary>
    Equilibrada,

    /// <summary>Percentual atual acima da faixa-alvo (concentração excessiva).</summary>
    Acima
}

/// <summary>
/// Ação sugerida em uma ordem de rebalanceamento.
/// </summary>
public enum AcaoRebalanceamento
{
    /// <summary>Aumentar a posição (compra) para elevar a alocação da classe.</summary>
    Aumentar,

    /// <summary>Reduzir a posição (venda) para diminuir a alocação da classe.</summary>
    Reduzir
}

/// <summary>
/// Alocação atual de uma classe de ativos comparada ao alvo configurado.
/// </summary>
public record AlocacaoClasseDto(
    string Classe,
    decimal PercentualAtual,
    decimal PercentualAlvo,
    SituacaoClasse Situacao);

/// <summary>
/// Sugestão de ajuste (compra ou venda) para aproximar a carteira dos alvos.
/// </summary>
public record SugestaoAjusteDto(
    string Ticker,
    AcaoRebalanceamento Acao,
    int Quantidade);

/// <summary>
/// Custo estimado para executar as sugestões de rebalanceamento.
/// </summary>
public record CustoRebalanceamentoDto(
    decimal CorretagemTotal,
    decimal IrEstimado,
    decimal CustoTotal);

/// <summary>
/// Resultado consolidado da análise de rebalanceamento da carteira.
/// </summary>
public record RebalanceamentoDto(
    IReadOnlyList<AlocacaoClasseDto> Alocacoes,
    IReadOnlyList<SugestaoAjusteDto> Sugestoes,
    CustoRebalanceamentoDto Custo);
