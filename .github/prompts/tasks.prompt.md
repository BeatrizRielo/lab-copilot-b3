---
mode: agent
description: "Deriva a lista de tarefas executáveis (tasks.md) a partir do plano da feature."
---

# /tasks — Gerar Tarefas

Passos:
1. Localize a feature ativa em `specs/NNN-nome-curto/` e leia `plan.md` + `spec.md`.
2. Crie `tasks.md` usando [.specify/templates/tasks-template.md](../../.specify/templates/tasks-template.md).
3. Escolha o modo conforme o estado do código:
   - **Feature nova:** ordene **Testes → Implementação → Refino**.
   - **Documentar comportamento existente:** ordene **Verificação/Regressão → Guardas de design → Validação contínua**.
4. Marque com `[P]` as tarefas paralelizáveis (arquivos independentes).
5. Cada tarefa deve ser pequena, verificável e apontar arquivo(s).
6. Preencha a tabela de **rastreabilidade** (tarefa → requisito → arquivo).

Ao final, sugira rodar `/implement` para executar as tarefas.
