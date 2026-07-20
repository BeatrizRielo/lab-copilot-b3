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
