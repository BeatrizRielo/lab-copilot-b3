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
- Regras tributárias reais da B3 (day trade, isenção de FII, faixa de isenção) — o
  cálculo de custo usa IR fixo de 15% sobre lucro na venda.
- Implementação do experimento A/B de rebalanceamento (planejada para fase futura).

## 8. Pendências
- Nenhuma nas regras vigentes (seções 3–6). A feature de rebalanceamento (seção 9) está
  **especificada e planejada** para o Sprint 25, ainda **não implementada**.

---

## 9. Feature — Sugestões de Rebalanceamento (Sprint 25)

**Status:** Proposto · **ADR:** [docs/adr/0006-sugestoes-rebalanceamento.md](../../docs/adr/0006-sugestoes-rebalanceamento.md)

### 9.1 Resumo
Orientar o investidor quando a carteira estiver **concentrada** fora da alocação-alvo por
classe (Ação, FII, ETF), sugerir ajustes de compra/venda e estimar o **custo** do
rebalanceamento (corretagem + IR). O algoritmo é simples e baseado em percentual-alvo
configurável; regras de negócio vivem em `Services/`.

### 9.2 Cenários de Usuário
Como **investidor da B3**, quero **saber quando minha carteira está concentrada** e
**quais ajustes fazer**, para **manter minha estratégia-alvo com custo consciente**.

Fluxos:
1. Abro o rebalanceamento e vejo que estou com 60% em ações (alvo 50% ±5%) — concentração sinalizada.
2. Recebo a sugestão "reduzir PETR4 e VALE3, aumentar MXRF11".
3. Vejo o custo estimado do rebalanceamento (corretagem + IR sobre o lucro das vendas).

### 9.3 Requisitos Funcionais
- **RF-006:** O sistema calcula o **percentual de cada classe** (Ação, FII, ETF) sobre o valor atual total da carteira.
- **RF-007:** O sistema **sinaliza concentração excessiva ou subalocação** quando a classe sai da faixa `[alvo − tolerância, alvo + tolerância]` (padrão Ações 50% / FII 30% / ETF 20%, tolerância ±5%).
- **RF-008:** O sistema **sugere ajustes**: reduzir ativos das classes acima do alvo e aumentar os das classes abaixo, priorizando o maior desvio primeiro.
- **RF-009:** O sistema **estima o custo do rebalanceamento**: corretagem fixa por ordem + IR de 15% sobre o lucro estimado das vendas.
- **RF-010:** O sistema expõe a sugestão via **`GET /api/portfolio/rebalanceamento`**.
- **RF-011 (fase futura):** O sistema permite comparar **sugestão automática vs. decisão manual** do usuário (experimento A/B) — fora do Sprint 25.

### 9.4 Regras de Negócio
- **RN-004:** Alocação-alvo e tolerância por classe são **configuráveis** (`appsettings.json`); padrão Ações 50% / FII 30% / ETF 20%, tolerância ±5%.
- **RN-005:** Custo de venda inclui **IR de 15% apenas sobre lucro positivo**: `lucro = (precoAtual − precoMedio) × qtdVendida`; se `lucro ≤ 0`, IR = 0.
- **RN-006:** A sugestão prioriza o **maior desvio** da classe em relação ao alvo; dentro da classe, o ativo de maior desvio de peso primeiro.
- **RN-007:** Nenhuma sugestão pode propor **vender mais do que a posição atual** (reaproveita RN-002).

### 9.5 Casos de Borda
- Carteira vazia → sem sugestões, custo zero, sem erro.
- Todas as classes dentro da faixa-alvo → "carteira equilibrada", sem ajustes.
- Uma única classe presente → sinaliza subalocação das classes ausentes.
- Valor atual total igual a zero → percentuais retornam 0 (sem divisão por zero).
- Venda sugerida com `lucro ≤ 0` → IR = 0 no custo estimado.

### 9.6 Critérios de Aceite
- [ ] Percentual por classe calculado sobre o valor atual total.
- [ ] Concentração/subalocação sinalizada conforme faixa-alvo configurável.
- [ ] Sugestão de reduzir/aumentar por maior desvio.
- [ ] Custo estimado = corretagem + IR 15% sobre lucro positivo das vendas.
- [ ] Endpoint `GET /api/portfolio/rebalanceamento` retorna a sugestão.
- [ ] `dotnet test` verde cobrindo RF-006..RF-010 e casos de borda.