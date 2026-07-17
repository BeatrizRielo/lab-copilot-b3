# 💼 Carteira B3 — Lab GitHub Copilot

Aplicação de exemplo para o **workshop de GitHub Copilot da B3**. Uma carteira de
investimentos simples (ações, FIIs e ETFs) usada para praticar as capacidades do
Copilot: **revisar código, gerar testes e refatorar**.

> Esta é a branch **base/solução** — código limpo, testes verdes e app funcionando.
> Durante o workshop, os participantes trabalham sobre uma branch com "defeitos propositais"
> (ver [`docs/gabarito-erros.md`](docs/gabarito-erros.md)).

---

## 🧱 Arquitetura

```
lab-copilot-b3/
├── backend/                     ASP.NET Core 9 Web API + EF Core (SQLite)
│   ├── src/PortfolioApi/
│   │   ├── Controllers/         Ativos, Ordens, Watchlist, Portfolio
│   │   ├── Services/            Regras de negócio (P&L, preço médio, validações)
│   │   ├── Models/              Ativo, Ordem, WatchlistItem
│   │   ├── Data/                PortfolioContext + Seed
│   │   ├── Dtos/                Contratos de entrada/saída
│   │   ├── Validation/          TickerValidator (padrão B3)
│   │   └── Middleware/          Tratamento de erros → HTTP 400/404
│   └── tests/PortfolioApi.Tests/  xUnit (23 testes)
└── frontend/                    React + Vite
    └── src/components/          Ativos, Ordens, Watchlist, Resumo
```

### Funcionalidades (3 CRUDs + resumo)
| Recurso | Endpoints | Regra de negócio |
|---------|-----------|------------------|
| **Ativos** | `GET/POST/PUT/DELETE /api/ativos` | validação de ticker B3, sem duplicidade |
| **Ordens** | `GET/POST/DELETE /api/ordens` | preço médio ponderado, venda ≤ posição |
| **Watchlist** | `GET/POST/PUT/DELETE /api/watchlist` | ticker válido, preço-alvo > 0 |
| **Resumo** | `GET /api/portfolio/resumo` | posição consolidada e P&L |

---

## ▶️ Como rodar

### Pré-requisitos
- [.NET SDK 9](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)

### Backend (porta 5000)
```bash
cd backend
dotnet run --project src/PortfolioApi
```
Swagger em: <http://localhost:5000/swagger>

### Frontend (porta 5173)
```bash
cd frontend
npm install
npm run dev
```
App em: <http://localhost:5173> (o Vite faz proxy de `/api` para o backend).

### Testes
```bash
cd backend
dotnet test
```

---

## 🎯 Roteiro do workshop
Veja o [**Guia do Participante**](docs/guia-participante.md) com os 4 blocos de exercícios
(revisar → testar → refatorar → evoluir) e os prompts sugeridos para o Copilot.

---

## 📌 Notas
- Banco **SQLite** recriado e populado (seed) a cada inicialização — sem configuração externa.
- Cotações são **simuladas** (`CotacaoSimuladaProvider`) para o cálculo de P&L.
