# Tarefas — [NOME DA FEATURE]

**Branch:** `[###-nome-curto]` · **Plano:** [./plan.md](./plan.md)

> Convenções:
> - `[P]` = pode rodar em paralelo (arquivos independentes).
> - Siga a ordem: **Testes → Implementação → Refino**.
> - Marque `[x]` ao concluir cada tarefa.

## Fase 1 — Testes (Test-First)
- [ ] T001 Escrever teste que prova a regra principal em `backend/tests/PortfolioApi.Tests/`.
- [ ] T002 [P] Escrever testes de casos de borda (inválido / limite / vazio).

## Fase 2 — Implementação
- [ ] T003 Implementar regra no service correspondente em `backend/src/PortfolioApi/Services/`.
- [ ] T004 Ajustar controller/DTO se necessário (apenas delegação).

## Fase 3 — Validação e Refino
- [ ] T005 Rodar `dotnet test` e garantir verde.
- [ ] T006 Rodar `./scripts/coverage.ps1` e revisar cobertura.
- [ ] T007 [P] Documentar métodos públicos alterados (XML doc).

## Rastreabilidade
| Tarefa | Requisito (RF/RN) | Arquivo |
|--------|-------------------|---------|
| T001 | | |
