# Tarefas — Sprint 25: Sugestões de Rebalanceamento

**Branch:** `feature/lab-gh-customization-SDD` · **Plano:** [../plan.md](../plan.md) (seção 8) · **Spec:** [../spec.md](../spec.md) (seção 9) · **ADR:** [0006](../../../docs/adr/0006-sugestoes-rebalanceamento.md) · **Status:** Planejado

> `[P]` = paralelizável. Esta feature **ainda não está implementada**; siga o fluxo
> **Test-First** (Artigo IV). Marque `[x]` ao concluir cada tarefa com `dotnet test` verde.

## Fase 0 — Configuração e Contratos
- [ ] T101 Adicionar seção `Rebalanceamento` (alvos por classe, tolerância, corretagem, IR) em `appsettings.json` e `appsettings.Development.json`.
- [ ] T102 [P] Criar DTOs em `Dtos/RebalanceamentoDtos.cs`: `AlocacaoClasseDto`, `SugestaoAjusteDto`, `CustoRebalanceamentoDto`, `RebalanceamentoDto`.

## Fase 1 — Testes Primeiro (xUnit)
- [ ] T103 Teste: percentual por classe calculado sobre o valor atual total (RF-006).
- [ ] T104 [P] Teste: sinalização de concentração/subalocação conforme faixa-alvo ±tolerância (RF-007 / RN-004).
- [ ] T105 [P] Teste: sugestão reduzir/aumentar priorizando maior desvio (RF-008 / RN-006).
- [ ] T106 [P] Teste: custo = corretagem + IR 15% sobre lucro positivo; IR = 0 quando lucro ≤ 0 (RF-009 / RN-005).
- [ ] T107 [P] Teste de bordas: carteira vazia, carteira equilibrada, classe única, valor total zero.
- [ ] T108 [P] Teste: sugestão nunca vende acima da posição atual (RN-007).

## Fase 2 — Implementação
- [ ] T109 Criar `Services/RebalanceamentoService.cs` (percentual por classe, detecção de desvio, sugestões, custo). Reusa `ICotacaoProvider`; sem duplicar P&L (Artigo V).
- [ ] T110 Registrar `RebalanceamentoService` no DI (`Program.cs`).
- [ ] T111 Adicionar endpoint `GET /api/portfolio/rebalanceamento` em `Controllers/PortfolioController.cs` (apenas delega — Artigo II).
- [ ] T112 [P] XML doc nos métodos públicos do service e nomes de domínio em português (Artigo I).

## Fase 3 — Validação Contínua
- [ ] T113 Rodar `dotnet test` e garantir verde (Artigo IV).
- [ ] T114 Rodar `./scripts/coverage.ps1` e revisar cobertura da nova feature.

## Fase Futura (fora do Sprint 25)
- [ ] T115 Experimento A/B: feature flag sugestão automática vs. manual, coleta de métricas e análise (RF-011 / ADR 0006).

## Rastreabilidade
| Tarefa | Requisito (RF/RN) | Arquivo |
|--------|-------------------|---------|
| T101 | RN-004 | `appsettings.json` |
| T102 | RF-006..RF-010 | `Dtos/RebalanceamentoDtos.cs` |
| T103 | RF-006 | `tests/PortfolioApi.Tests/RebalanceamentoServiceTests.cs` |
| T104 | RF-007 / RN-004 | `tests/PortfolioApi.Tests/RebalanceamentoServiceTests.cs` |
| T105 | RF-008 / RN-006 | `tests/PortfolioApi.Tests/RebalanceamentoServiceTests.cs` |
| T106 | RF-009 / RN-005 | `tests/PortfolioApi.Tests/RebalanceamentoServiceTests.cs` |
| T107 | RF-006 (bordas) | `tests/PortfolioApi.Tests/RebalanceamentoServiceTests.cs` |
| T108 | RN-007 | `tests/PortfolioApi.Tests/RebalanceamentoServiceTests.cs` |
| T109 | RF-006..RF-009 | `Services/RebalanceamentoService.cs` |
| T110 | RF-010 | `Program.cs` |
| T111 | RF-010 | `Controllers/PortfolioController.cs` |
| T115 | RF-011 | (fase futura) |