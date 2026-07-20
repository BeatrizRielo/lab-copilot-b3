# Constituição do Projeto — Carteira B3 (Lab GitHub Copilot)

> Princípios não-negociáveis que guiam a Spec-Driven Development (SDD) neste repositório.
> Todo `spec.md`, `plan.md` e `tasks.md` deve respeitar esta constituição.

## Artigo I — Domínio em Português
- Código, nomes de domínio e mensagens de erro em **português** (ex.: `Ativo`, `Ordem`, `PrecoMedio`).
- Termos de negócio seguem o padrão da B3 (ações, FIIs, ETFs).

## Artigo II — Camadas e Responsabilidades
- Regras de negócio vivem em **`Services/`**; controllers apenas delegam.
- Validação de ticker sempre via **`TickerValidator`** (padrão B3: 4 letras + 1–2 dígitos).
- Erros de negócio usam `RegraNegocioException` (→ HTTP 400) ou `NaoEncontradoException` (→ 404).

## Artigo III — Regras de Negócio Críticas
- **Preço médio** é ponderado na **compra**; na **venda** apenas reduz a quantidade.
- **Venda não pode exceder** a posição atual do ativo.
- **Resultado (P&L)** = valor atual − valor investido; percentual sobre o **investido**.

## Artigo IV — Testes Primeiro (Test-First)
- Toda regra de negócio nova ou corrigida deve ter teste xUnit.
- Testes usam `TestDb.CreateContext()` (SQLite in-memory isolado por teste).
- Casos de borda são obrigatórios: ticker inválido, quantidade/preço ≤ 0, venda acima da posição, carteira vazia.
- `dotnet test` deve ficar **verde** antes de concluir qualquer tarefa.

## Artigo V — Qualidade e Simplicidade
- Evitar duplicação (DRY) e over-engineering.
- Preferir clareza de nomes e documentação XML em métodos públicos.
- Nenhuma mudança pode quebrar testes existentes.

## Artigo VI — Rastreabilidade SDD
- Fluxo obrigatório: **Constituição → Spec → Plan → Tasks → Implementação**.
- Cada feature vive em `specs/NNN-nome-curto/`.
- Nenhum código é escrito sem uma `spec.md` aprovada.

## Artigo VII — Decisões de Arquitetura (ADR)
- Decisões de arquitetura relevantes são registradas como **ADR** em `docs/adr/`.
- Todo `plan.md` que tomar uma decisão arquitetural deve criar ou referenciar um ADR.
- ADRs são imutáveis: para mudar uma decisão, crie um novo ADR marcando o anterior como **Substituído**.

---

**Versão:** 1.0.0 · **Ratificada em:** 2026-07-20
