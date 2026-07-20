# Architecture Decision Records (ADR)

Registros de decisões de arquitetura da **Carteira B3**. Cada ADR documenta uma
decisão relevante, seu contexto e consequências.

> Formato baseado no modelo de Michael Nygard.
> Para criar um novo ADR, copie [0000-template.md](0000-template.md) e incremente o número.

## Índice
| # | Título | Status |
|---|--------|--------|
| [0001](0001-camadas-services.md) | Regras de negócio concentradas em Services | Aceito |
| [0002](0002-sqlite-ef-core.md) | SQLite + EF Core como persistência | Aceito |
| [0003](0003-validacao-ticker-centralizada.md) | Validação de ticker centralizada no TickerValidator | Aceito |
| [0004](0004-spec-driven-development.md) | Adoção de Spec-Driven Development (Spec Kit) | Aceito |
| [0005](0005-preco-medio-e-pl.md) | Preço médio ponderado e cálculo de P&L | Aceito |
| [0006](0006-sugestoes-rebalanceamento.md) | Sugestões de rebalanceamento da carteira | Proposto |

## Status possíveis
- **Proposto** — em discussão.
- **Aceito** — decisão em vigor.
- **Substituído** — trocado por outro ADR (referencie qual).
- **Depreciado** — não se aplica mais.
