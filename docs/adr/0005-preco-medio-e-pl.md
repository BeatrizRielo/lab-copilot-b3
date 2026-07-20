# 0005 — Preço médio ponderado e cálculo de P&L

- **Status:** Aceito
- **Data:** 2026-07-20
- **Decisores:** Time do Lab B3
- **Relacionado:** [Constituição — Artigo III](../../.specify/memory/constitution.md), [specs/001-carteira-regras](../../specs/001-carteira-regras/spec.md)

## Contexto
A carteira precisa refletir de forma consistente o custo de aquisição dos ativos e o
resultado (P&L) do investidor. Compras sucessivas do mesmo ativo a preços diferentes
exigem uma convenção clara de custo; vendas parciais não podem distorcer esse custo.

## Decisão
- **Preço médio ponderado na compra:** ao comprar, o novo preço médio é
  `(custoAtual + custoNovo) / quantidadeTotal`, com guarda para `quantidadeTotal == 0`.
- **Venda mantém o preço médio:** a venda apenas reduz a quantidade; não altera o
  preço médio (padrão contábil para apuração posterior de resultado).
- **P&L absoluto:** `resultado = valorAtual − valorInvestido`.
- **P&L percentual:** calculado sobre o **valor investido**, arredondado a 2 casas
  (`Math.Round(..., 2)`), retornando 0 quando o investido é 0 (sem divisão por zero).

## Alternativas Consideradas
- **PEPS/FIFO (lotes):** mais preciso para tributação real, porém complexo e fora do
  escopo do lab. Rejeitado por ora.
- **Percentual sobre o valor atual:** distorce a leitura de rentabilidade e inverte a
  base de compração. Rejeitado.

## Consequências
### Positivas
- Cálculo simples, determinístico e fácil de testar.
- Resultado e rentabilidade consistentes com a intuição do investidor.

### Negativas / Trade-offs
- Não modela lotes individuais nem regras tributárias específicas (day trade, etc.).
