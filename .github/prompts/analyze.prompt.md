---
mode: agent
description: "Analisa a consistência entre spec, plan, tasks, constituição e ADRs antes de implementar."
---

# /analyze — Analisar Consistência (SDD)

> Análise somente-leitura. Não altere código de produção.

Passos:
1. Localize a feature ativa em `specs/NNN-nome-curto/` e leia `spec.md`, `plan.md`, `tasks.md`.
2. Leia a [constituição](../../.specify/memory/constitution.md) e os ADRs em `docs/adr/`.
3. Verifique e reporte:
   - **Cobertura:** todo RF/RN da spec tem tarefa correspondente em `tasks.md`?
   - **Rastreabilidade:** a tabela tarefa → requisito → arquivo está completa e correta?
   - **Gate da constituição:** o `plan.md` respeita os Artigos I–VII?
   - **ADR:** decisões de arquitetura do `plan.md` estão registradas em `docs/adr/`?
   - **Contradições:** divergências entre spec, plan e tasks (status, regras, escopo).
4. Classifique cada achado como **Crítico / Atenção / Sugestão**.

Ao final, entregue um relatório com os achados e a ação recomendada para cada um.
