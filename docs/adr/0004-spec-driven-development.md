# 0004 — Adoção de Spec-Driven Development (Spec Kit)

- **Status:** Aceito
- **Data:** 2026-07-20
- **Decisores:** Time do Lab B3
- **Relacionado:** [.specify/memory/constitution.md](../../.specify/memory/constitution.md), [specs/001-carteira-regras](../../specs/001-carteira-regras/spec.md)

## Contexto
O lab explora o uso do GitHub Copilot de forma disciplinada. Escrever código direto do
prompt, sem intenção documentada, produz resultados inconsistentes e difíceis de revisar.

## Decisão
Adotar **Spec-Driven Development** com a estrutura do **GitHub Spec Kit**:
`Constituição → Spec → Plan → Tasks → Implementação`. Os comandos são expostos como
prompts do Copilot em `.github/prompts/` (`/specify`, `/plan`, `/tasks`, `/implement`,
`/constitution`). Cada feature vive em `specs/NNN-nome-curto/`.

## Alternativas Consideradas
- **Prompt-and-code direto:** rápido, mas sem rastreabilidade nem gate de qualidade. Rejeitado.
- **Documentação pesada (BRD/PRD tradicionais):** completa, porém lenta e desalinhada
  do fluxo com IA. Rejeitado para o escopo do lab.

## Consequências
### Positivas
- Rastreabilidade requisito → tarefa → arquivo.
- Gate da constituição força qualidade e Test-First.
- Fluxo repetível e ensinável no workshop.

### Negativas / Trade-offs
- Overhead inicial para escrever spec/plan antes de codar.
