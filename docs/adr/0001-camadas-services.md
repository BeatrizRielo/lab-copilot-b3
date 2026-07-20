# 0001 — Regras de negócio concentradas em Services

- **Status:** Aceito
- **Data:** 2026-07-20
- **Decisores:** Time do Lab B3
- **Relacionado:** [Constituição — Artigo II](../../.specify/memory/constitution.md)

## Contexto
A aplicação expõe endpoints REST para operações de carteira (ordens, resumo, watchlist).
Sem uma fronteira clara, a lógica de cálculo (preço médio, P&L, validações) tende a
vazar para os controllers, dificultando testes e reuso.

## Decisão
Toda regra de negócio vive na camada **`Services/`**. Os **controllers apenas delegam**:
recebem a requisição, chamam o service e retornam o resultado. Erros de negócio são
sinalizados por exceções de domínio (`RegraNegocioException`, `NaoEncontradoException`).

## Alternativas Consideradas
- **Lógica nos controllers:** mais rápido de escrever, mas acopla HTTP à regra e
  dificulta testes unitários. Rejeitado.
- **Camada de aplicação separada (CQRS/MediatR):** poderoso, porém over-engineering
  para o escopo do lab. Rejeitado por ora.

## Consequências
### Positivas
- Regras testáveis isoladamente com xUnit e `TestDb.CreateContext()`.
- Controllers finos e uniformes.

### Negativas / Trade-offs
- Exige disciplina para não reintroduzir lógica nos controllers.
