using PortfolioApi.Models;

namespace PortfolioApi.Services;

/// <summary>
/// Configuração de rebalanceamento vinda de appsettings (seção "Rebalanceamento").
/// </summary>
public class RebalanceamentoOptions
{
    /// <summary>Percentual-alvo por classe de ativo (ex.: "Acao" = 50).</summary>
    public Dictionary<string, decimal> AlvosPorClasse { get; set; } = new();

    /// <summary>Tolerância percentual em torno do alvo antes de sinalizar desvio.</summary>
    public decimal ToleranciaPercentual { get; set; } = 5m;

    /// <summary>Corretagem fixa cobrada por ordem executada.</summary>
    public decimal CorretagemPorOrdem { get; set; }

    /// <summary>Alíquota de IR sobre o lucro na venda (fração, ex.: 0.15 = 15%).</summary>
    public decimal AliquotaIr { get; set; } = 0.15m;

    /// <summary>
    /// Valida a coerência da configuração de rebalanceamento. Garante que os alvos
    /// somem 100%, cubram todas as classes de ativo e não usem chaves inválidas —
    /// evitando distorções silenciosas no cálculo de alocação.
    /// </summary>
    /// <exception cref="InvalidOperationException">Quando a configuração é incoerente.</exception>
    public void Validate()
    {
        if (AlvosPorClasse.Count == 0)
            throw new InvalidOperationException(
                "Rebalanceamento: 'AlvosPorClasse' não pode estar vazio.");

        // Toda chave deve corresponder a um TipoAtivo válido.
        var chavesInvalidas = AlvosPorClasse.Keys
            .Where(k => !Enum.TryParse<TipoAtivo>(k, out _))
            .ToList();
        if (chavesInvalidas.Count > 0)
            throw new InvalidOperationException(
                $"Rebalanceamento: classe(s) inválida(s) em 'AlvosPorClasse': {string.Join(", ", chavesInvalidas)}. " +
                $"Valores aceitos: {string.Join(", ", Enum.GetNames<TipoAtivo>())}.");

        // Toda classe de ativo existente deve ter um alvo, para nenhuma escapar do rebalanceamento.
        var classesSemAlvo = Enum.GetNames<TipoAtivo>()
            .Where(nome => !AlvosPorClasse.ContainsKey(nome))
            .ToList();
        if (classesSemAlvo.Count > 0)
            throw new InvalidOperationException(
                $"Rebalanceamento: classe(s) sem alvo configurado: {string.Join(", ", classesSemAlvo)}. " +
                "Configure um alvo para todas as classes de ativo.");

        // A soma dos alvos deve ser 100% (com tolerância mínima para arredondamento).
        var soma = AlvosPorClasse.Values.Sum();
        if (Math.Abs(soma - 100m) > 0.01m)
            throw new InvalidOperationException(
                $"Rebalanceamento: a soma dos alvos por classe deve ser 100%, mas é {soma}%.");
    }
}
