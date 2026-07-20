---
mode: agent
description: "Cria um novo Architecture Decision Record (ADR) a partir de uma decisão de arquitetura."
---

# /adr — Registrar Decisão de Arquitetura

Decisão a registrar: **${input:decisao:Descreva a decisão de arquitetura}**

Passos:
1. Leia o índice em [docs/adr/README.md](../../docs/adr/README.md) e escolha o próximo número (`NNNN`).
2. Crie `docs/adr/NNNN-nome-curto.md` usando [docs/adr/0000-template.md](../../docs/adr/0000-template.md).
3. Preencha Contexto, Decisão, Alternativas Consideradas e Consequências (positivas e trade-offs).
4. Adicione a linha correspondente ao índice em `docs/adr/README.md`.
5. Se esta decisão **substitui** outra, marque o ADR anterior como **Substituído** e referencie o novo (Constituição — Artigo VII).
6. Se veio de um `plan.md`, referencie o ADR na seção "Decisões de Arquitetura (ADR)".

Escreva em **português**, alinhado ao domínio B3.
