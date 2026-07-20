---
mode: agent
description: "Gera o plano de implementação (plan.md) a partir da spec aprovada da feature."
---

# /plan — Criar Plano de Implementação

Contexto opcional do usuário: **${input:contexto:Restrições ou preferências técnicas (opcional)}**

Passos:
1. Localize a feature ativa em `specs/NNN-nome-curto/` (a mais recente ou a indicada).
2. Leia a `spec.md` e a [constituição](../../.specify/memory/constitution.md).
3. Crie `plan.md` usando [.specify/templates/plan-template.md](../../.specify/templates/plan-template.md).
4. **Preencha o gate da constituição** e justifique qualquer exceção.
5. Liste os arquivos prováveis a serem tocados no backend/frontend.
6. Defina a estratégia de testes (Test-First) com casos de borda.
7. Se houver decisão de arquitetura, **crie ou referencie um ADR** em `docs/adr/` (Constituição — Artigo VII) e cite na seção "Decisões de Arquitetura (ADR)".

Não escreva código de produção aqui. Ao final, sugira rodar `/tasks`.
