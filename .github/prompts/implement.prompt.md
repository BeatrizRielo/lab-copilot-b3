---
mode: agent
description: "Executa as tarefas de tasks.md seguindo Test-First até os testes ficarem verdes."
---

# /implement — Implementar Tarefas

Passos:
1. Localize a feature ativa em `specs/NNN-nome-curto/` e leia `tasks.md`, `plan.md`, `spec.md`.
2. Respeite a [constituição](../../.specify/memory/constitution.md) — em especial o **Artigo IV (Testes Primeiro)**.
3. Execute as tarefas **na ordem**, marcando `[x]` conforme conclui cada uma:
   - Fase 1: escreva os testes primeiro (feature nova: devem falhar; comportamento existente: servem como regressão).
   - Fase 2: implemente as regras nos `Services/` (quando aplicável).
   - Fase 3: rode `dotnet test` e `./scripts/coverage.ps1`.
4. Não pule tarefas nem invente escopo fora da `spec.md`.
5. Se encontrar `[NEEDS CLARIFICATION]`, pare e pergunte.

Ao final, resuma o que foi implementado e o estado dos testes (verde/vermelho).
