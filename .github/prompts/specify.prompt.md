---
mode: agent
description: "Cria a especificação (spec.md) de uma nova feature a partir de uma descrição em linguagem natural."
---

# /specify — Criar Especificação

Você está no fluxo **Spec-Driven Development** deste repositório.

A descrição da feature fornecida pelo usuário é: **${input:descricao:Descreva a feature}**

Passos:
1. Leia a constituição em [.specify/memory/constitution.md](../../.specify/memory/constitution.md) e respeite todos os artigos.
2. Escolha o próximo número de feature (`NNN`) olhando a pasta `specs/`.
3. Crie `specs/NNN-nome-curto/spec.md` usando [.specify/templates/spec-template.md](../../.specify/templates/spec-template.md).
4. Foque no **O QUÊ** e no **PORQUÊ** — sem stack, sem código.
5. Marque toda ambiguidade com `[NEEDS CLARIFICATION: ...]`.
6. Escreva em **português**, alinhado ao domínio B3.

Ao final, liste as pendências de clarificação e sugira rodar `/plan`.
