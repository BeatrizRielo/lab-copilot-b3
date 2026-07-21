# Plano de Implementação — Regras da Carteira B3

**Branch:** `feature/lab-gh-custumization-SDD` · **Spec:** [./spec.md](./spec.md) · **Status:** Implementado

## Verificação da Constituição (Gate)
- [x] Domínio em português (Artigo I)
- [x] Regras em `Services/`, controllers só delegam (Artigo II)
- [x] Regras de negócio críticas preservadas (Artigo III)
- [x] Testes primeiro, com casos de borda (Artigo IV)
- [x] Sem duplicação / over-engineering (Artigo V)

## 1. Contexto Técnico
- **Stack:** ASP.NET Core 9 Web API, EF Core (SQLite), xUnit · React + Vite.
- **Arquivos que implementam estas regras:**
  - `backend/src/PortfolioApi/Services/OrdemService.cs` (preço médio, controle de posição)
  - `backend/src/PortfolioApi/Services/PortfolioService.cs` (P&L e percentual)
  - `backend/src/PortfolioApi/Services/WatchlistService.cs` (validação via `TickerValidator`)
  - `backend/tests/PortfolioApi.Tests/` (testes de regressão)

## 2. Abordagem (implementação vigente)
1. **Preço médio** (`OrdemService.AtualizarPosicao`): na compra, `(custoAtual + custoNovo) / quantidadeTotal`, com guarda de divisão por zero; na venda, apenas reduz a quantidade mantendo o preço médio.
2. **Controle de posição** (`OrdemService.CriarAsync`): venda acima da posição lança `RegraNegocioException`; `ValidarEntrada` rejeita quantidade/preço ≤ 0.
3. **P&L** (`PortfolioService.ObterResumoAsync`): `resultado = valorAtual - valorInvestido`; percentual sobre o investido com `Math.Round(..., 2)` e guarda de investido zero.
4. **Validação de ticker** centralizada em `TickerValidator` (sem regex inline nos services).

## 3. Modelo de Dados
Sem mudança de schema. Entidades `Ativo`, `Ordem` permanecem.

## 4. Contratos / Endpoints
| Método | Rota | Entrada | Saída | Regra |
|--------|------|---------|-------|-------|
| POST | `/api/ordens` | `OrdemCreateDto` | `OrdemDto` | RN-001, RN-002 |
| GET | `/api/portfolio/resumo` | — | `ResumoCarteiraDto` | RN-003 |

## 5. Estratégia de Testes
- `OrdemService`: preço médio ponderado, venda > posição.
- `PortfolioService`: sinal do resultado, denominador do percentual.
- Casos de borda: quantidade/preço ≤ 0, ticker inválido, carteira vazia, investido zero.

## 6. Riscos
- Nenhum risco aberto: comportamento implementado e coberto por testes de regressão.

## 7. Decisões de Arquitetura (ADR)
- [docs/adr/0005-preco-medio-e-pl.md](../../docs/adr/0005-preco-medio-e-pl.md) — preço médio ponderado e cálculo de P&L.
- [docs/adr/0003-validacao-ticker-centralizada.md](../../docs/adr/0003-validacao-ticker-centralizada.md) — validação de ticker centralizada.
- [docs/adr/0006-sugestoes-rebalanceamento.md](../../docs/adr/0006-sugestoes-rebalanceamento.md) — sugestões de rebalanceamento (Sprint 25).

---

## 8. Feature — Sugestões de Rebalanceamento (Sprint 25)

**Spec:** [./spec.md](./spec.md) seção 9 · **ADR:** [0006](../../docs/adr/0006-sugestoes-rebalanceamento.md) · **Status:** Planejado

### 8.1 Verificação da Constituição (Gate)
- [ ] Domínio em português: `RebalanceamentoService`, `SugestaoAjuste`, `CustoRebalanceamento` (Artigo I).
- [ ] Regras no novo `RebalanceamentoService`; controller apenas delega (Artigo II).
- [ ] Reaproveita preço médio/cotação existentes; sem duplicar P&L (Artigo V).
- [ ] Testes primeiro com casos de borda (Artigo IV).
- [ ] Decisão registrada no ADR 0006 (Artigo VII).

### 8.2 Abordagem
1. **Novo `RebalanceamentoService`** (`Services/`): calcula percentual por classe
   (`TipoAtivo`) sobre o valor atual total (reusa `ICotacaoProvider` como em `PortfolioService`).
2. **Detecção de desvio:** compara cada classe com a faixa `[alvo − tolerância, alvo + tolerância]`
   lida da configuração; marca `Acima`, `Abaixo` ou `Equilibrada`.
3. **Sugestão:** ordena classes por maior desvio; para classes `Acima`, sugere reduzir os
   ativos de maior peso; para `Abaixo`, sugere aumentar. Nenhuma venda excede a posição (RN-002/RN-007).
4. **Custo estimado:** corretagem fixa por ordem sugerida + IR de 15% sobre `lucro` positivo
   das vendas (`(precoAtual − precoMedio) × qtdVendida`).
5. **Endpoint:** `GET /api/portfolio/rebalanceamento` no `PortfolioController` (apenas delega).
6. **A/B test (RF-011):** fora do Sprint 25; registrado no ADR 0006 como fase futura.

### 8.3 Configuração (`appsettings.json`)
```jsonc
"Rebalanceamento": {
  "AlvosPorClasse": { "Acao": 50, "FII": 30, "ETF": 20 },
  "ToleranciaPercentual": 5,
  "CorretagemPorOrdem": 0.0,
  "AliquotaIr": 0.15
}
```

### 8.4 Contratos / DTOs
| Método | Rota | Saída | Regra |
|--------|------|-------|-------|
| GET | `/api/portfolio/rebalanceamento` | `RebalanceamentoDto` | RF-006..RF-010 |

- `RebalanceamentoDto`: lista de `AlocacaoClasseDto` (classe, percentualAtual, percentualAlvo, situação), lista de `SugestaoAjusteDto` (ticker, ação reduzir/aumentar, quantidade), e `CustoRebalanceamentoDto` (corretagemTotal, irEstimado, custoTotal).

### 8.5 Estratégia de Testes
- `RebalanceamentoService`: percentual por classe, detecção de desvio, ordenação por maior desvio, cálculo de custo (IR só sobre lucro positivo).
- Casos de borda: carteira vazia, carteira equilibrada, classe única, valor total zero, venda com lucro ≤ 0.
- `TestDb.CreateContext()` (SQLite in-memory) conforme Artigo IV.

### 8.6 Riscos
- IR fixo de 15% é aproximação; não cobre isenções reais (documentado no ADR 0006).
- Ausência do experimento A/B impede medir o valor da sugestão automática até fase futura.