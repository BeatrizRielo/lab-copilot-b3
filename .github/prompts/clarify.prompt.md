---
mode: agent
description: "Faz perguntas de clarificação para eliminar ambiguidades da spec antes do planejamento."
---

# /clarify — Clarificar Especificação

Passos:
1. Localize a feature ativa em `specs/NNN-nome-curto/` e leia a `spec.md`.
2. Identifique todos os pontos marcados com `[NEEDS CLARIFICATION: ...]` e demais ambiguidades
   (regras de negócio vagas, casos de borda não definidos, entradas/saídas incertas).
3. Faça ao usuário perguntas objetivas, uma a uma, priorizando o que mais afeta o design.
4. Registre cada resposta **atualizando a `spec.md`**: remova o marcador e escreva a decisão.
5. Não invente respostas; se algo permanecer indefinido, mantenha o marcador.

Respeite a [constituição](../../.specify/memory/constitution.md). Ao final,
confirme que não há mais `[NEEDS CLARIFICATION]` e sugira rodar `/plan`.
