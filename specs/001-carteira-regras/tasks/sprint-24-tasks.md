# Tarefas — Regras da Carteira B3

**Branch:** `feature/lab-gh-custumization-SDD` · **Plano:** [./plan.md](./plan.md) · **Status:** Implementado

> `[P]` = paralelizável · Estas regras **já estão implementadas**; as tarefas abaixo
> **verificam e protegem** o comportamento vigente (regressão). Marque `[x]` ao confirmar.

## Fase 1 — Verificação das Regras (Regressão)
- [ ] T001 Confirmar preço médio ponderado após duas compras (`OrdemService`).
- [ ] T002 [P] Confirmar que venda maior que a posição lança `RegraNegocioException`.
- [ ] T003 [P] Confirmar sinal do resultado (`valorAtual - valorInvestido`) no `PortfolioService`.
- [ ] T004 [P] Confirmar percentual sobre o investido, arredondado a 2 casas.
- [ ] T005 [P] Confirmar bordas: quantidade/preço ≤ 0, ticker inválido, carteira vazia, investido zero.

## Fase 2 — Guardas de Design (já atendidas)
- [x] T006 Validação de ticker centralizada em `TickerValidator` (sem regex inline).
- [x] T007 Regras de negócio em `Services/`; controllers apenas delegam.
- [x] T008 Métodos públicos documentados (XML doc) e nomes claros em `OrdemService`.

## Fase 3 — Validação Contínua
- [ ] T009 Rodar `dotnet test` e garantir verde.
- [ ] T010 Rodar `./scripts/coverage.ps1` e revisar cobertura.

## Rastreabilidade
| Tarefa | Requisito (RF/RN) | Arquivo |
|--------|-------------------|---------|
| T001 | RF-001 / RN-001 | `Services/OrdemService.cs` |
| T002 | RF-002 / RN-002 | `Services/OrdemService.cs` |
| T003 | RF-003 | `Services/PortfolioService.cs` |
| T004 | RF-004 / RN-003 | `Services/PortfolioService.cs` |
| T005 | RF-005 | `Services/OrdemService.cs`, `Services/WatchlistService.cs` |
| T006 | Artigo V | `Services/WatchlistService.cs`, `Validation/TickerValidator.cs` |
| T007 | Artigo II | `Controllers/OrdensController.cs` |
