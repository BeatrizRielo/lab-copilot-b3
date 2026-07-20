# 0003 — Validação de ticker centralizada no TickerValidator

- **Status:** Aceito
- **Data:** 2026-07-20
- **Decisores:** Time do Lab B3
- **Relacionado:** [Constituição — Artigos II e V](../../.specify/memory/constitution.md)

## Contexto
Tickers da B3 seguem um padrão (4 letras + 1–2 dígitos, ex.: `PETR4`, `MXRF11`).
A validação por regex tende a ser copiada em múltiplos services (ordens, watchlist),
gerando duplicação e risco de divergência de regra.

## Decisão
Centralizar a validação e normalização de ticker em **`TickerValidator`** (padrão
`^[A-Z]{4}\d{1,2}$`). Nenhum service deve reimplementar a regex inline; todos
consomem `TickerValidator.IsValid` / normalização.

## Alternativas Consideradas
- **Regex inline por service:** simples no curto prazo, mas duplicado e frágil. Rejeitado.
- **Validação via DataAnnotations no DTO:** cobre entrada HTTP, mas não protege regras
  chamadas internamente. Complementar, não substituto.

## Consequências
### Positivas
- Fonte única de verdade para o formato de ticker (DRY).
- Fácil evoluir o padrão (ex.: novos sufixos) em um só lugar.

### Negativas / Trade-offs
- Exige que novos services consumam sempre o `TickerValidator` em vez de recriar a regex.
