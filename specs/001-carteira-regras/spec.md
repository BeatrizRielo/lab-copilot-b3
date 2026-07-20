# Especificação de Feature — Regras da Carteira B3

**Branch:** `feature/lab-gh-custumization-SDD` · **Criado em:** 2026-07-20 · **Status:** Implementado

## 1. Resumo
Este documento especifica, de forma viva e rastreável, as regras vigentes da carteira
de investimentos (ações, FIIs e ETFs da B3): cálculo de preço médio, controle de posição,
validações de entrada e apuração de resultado (P&L). O comportamento aqui descrito está
**implementado e coberto por testes** no backend, e serve como fonte de verdade para
evoluções futuras conduzidas via Spec-Driven Development.

## 2. Cenários de Usuário (User Stories)
### História principal
Como **investidor da B3**, quero **ver o resultado correto da minha carteira** para
**tomar decisões confiáveis de compra e venda**.

### Fluxos
1. Registro uma segunda compra de um ativo e o preço médio é recalculado corretamente.
2. Tento vender mais do que possuo e a operação é bloqueada com mensagem clara.
3. Abro o resumo e vejo lucro (verde) quando o valor atual supera o investido.

## 3. Requisitos Funcionais
- **RF-001:** O sistema calcula o preço médio ponderado pela **quantidade total** após compras sucessivas.
- **RF-002:** O sistema **bloqueia vendas** cuja quantidade exceda a posição atual.
- **RF-003:** O sistema calcula o resultado como **valor atual − valor investido**.
- **RF-004:** O sistema calcula o percentual de rentabilidade sobre o **valor investido**, arredondado a 2 casas.
- **RF-005:** O sistema rejeita quantidade ou preço menor ou igual a zero.

## 4. Regras de Negócio
- **RN-001:** Preço médio é ponderado apenas na compra; a venda só reduz a quantidade, mantendo o preço médio.
- **RN-002:** Venda não pode exceder a posição atual do ativo.
- **RN-003:** Percentual de P&L usa o total investido como denominador (0 quando investido é 0).

## 5. Casos de Borda
- Venda maior que a posição → erro de regra de negócio.
- Quantidade ou preço ≤ 0 → rejeitado.
- Ticker fora do padrão B3 → rejeitado (`TickerValidator`).
- Carteira vazia → totais zerados, sem erro.
- Valor investido igual a zero → percentual retorna 0 (sem divisão por zero).

## 6. Critérios de Aceite
- [x] Preço médio correto após duas compras.
- [x] Venda acima da posição lança exceção.
- [x] Resumo mostra resultado positivo quando a carteira está em alta.
- [x] Percentual calculado sobre o investido, com 2 casas.
- [x] `dotnet test` verde cobrindo as regras acima.

## 7. Fora de Escopo
- Integração com feed real de cotações (segue simulado via `ICotacaoProvider`).
- Autenticação/multiusuário.

## 8. Pendências
- Nenhuma. Comportamento implementado e alinhado a esta especificação.
