# Instruções do Copilot — Carteira B3

Projeto de workshop: **carteira de investimentos** (ações, FIIs, ETFs da B3).

## Stack
- **Backend:** ASP.NET Core 9 Web API, EF Core (SQLite), xUnit.
- **Frontend:** React + Vite (JavaScript).

## Convenções
- Código, nomes de domínio e mensagens de erro em **português** (ex.: `Ativo`, `Ordem`, `PrecoMedio`).
- Regras de negócio ficam em **`Services/`**; controllers apenas delegam.
- Validação de ticker sempre via **`TickerValidator`** (padrão B3: 4 letras + 1–2 dígitos).
- Erros de negócio lançam `RegraNegocioException` (→ HTTP 400) ou `NaoEncontradoException` (→ 404).
- Testes usam `TestDb.CreateContext()` (SQLite in-memory isolado por teste).

## Regras de negócio importantes
- **Preço médio** é ponderado na **compra**; na **venda** apenas reduz a quantidade.
- **Venda não pode exceder** a posição atual do ativo.
- **Resultado (P&L)** = valor atual − valor investido; percentual sobre o **investido**.

## Ao gerar testes
- Prefira `Theory`/`InlineData` para casos de borda.
- Cubra: ticker inválido, quantidade/preço ≤ 0, venda acima da posição, carteira vazia.
