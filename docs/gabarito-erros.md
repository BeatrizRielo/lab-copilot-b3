# 🐛 Gabarito de Erros — Defeitos Propositais do Lab

> **Uso:** este documento é o **mapa dos defeitos** que serão injetados na branch de
> exercícios (ex.: `feature/lab`). A branch base/`main` permanece **limpa e com testes verdes**.
> Quando quiser, peça ao Copilot: *"injete os defeitos D1..D8 conforme o gabarito"* e comite
> como uma feature branch.
>
> ⚠️ **Regra:** todos os defeitos mantêm o projeto **compilando e executável** — são bugs de
> **lógica e qualidade**, não erros de compilação. Assim os participantes conseguem rodar a app.

**Legenda de blocos:** 🔍 A = Revisar · ✅ B = Testar · 🔧 C = Refatorar

| ID | Categoria | Bloco | Arquivo |
|----|-----------|-------|---------|
| D1 | Bug de cálculo (preço médio) | 🔍 A / ✅ B | `Services/OrdemService.cs` |
| D2 | Bug de cálculo (P&L invertido) | 🔍 A / ✅ B | `Services/PortfolioService.cs` |
| D3 | Regra ausente (overselling) | 🔍 A / ✅ B | `Services/OrdemService.cs` |
| D4 | Bug de percentual (denominador) | 🔍 A | `Services/PortfolioService.cs` |
| D5 | Cobertura de testes removida | ✅ B | `tests/PortfolioApi.Tests/` |
| D6 | Código duplicado (validação) | 🔧 C | `Services/WatchlistService.cs` |
| D7 | Lógica de negócio no controller | 🔧 C | `Controllers/OrdensController.cs` |
| D8 | Nomes ruins + sem documentação | 🔧 C | `Services/OrdemService.cs` |

---

## D1 — Preço médio ponderado calculado errado (COMPRA)

**Arquivo:** `backend/src/PortfolioApi/Services/OrdemService.cs` → método `AtualizarPosicao`

**Código CORRETO (base):**
```csharp
var novaQuantidade = ativo.Quantidade + ordem.Quantidade;
ativo.PrecoMedio = novaQuantidade == 0 ? 0 : (custoAtual + custoNovo) / novaQuantidade;
```

**Código COM DEFEITO (injetar):**
```csharp
var novaQuantidade = ativo.Quantidade + ordem.Quantidade;
// BUG: divide pelo volume da nova ordem em vez da quantidade total
ativo.PrecoMedio = (custoAtual + custoNovo) / ordem.Quantidade;
```

**Sintoma:** após uma segunda compra, o preço médio fica absurdamente alto.
**Correção esperada:** dividir por `novaQuantidade` (com guarda contra zero).
**Pega pelo teste:** `Compra_RecalculaPrecoMedioPonderado`.

---

## D2 — Resultado (P&L) com sinal invertido

**Arquivo:** `backend/src/PortfolioApi/Services/PortfolioService.cs`

**Código CORRETO (base):**
```csharp
var resultado = valorAtual - valorInvestido;
```

**Código COM DEFEITO (injetar):**
```csharp
// BUG: sinal invertido — mostra lucro como prejuízo e vice-versa
var resultado = valorInvestido - valorAtual;
```

**Sintoma:** carteira em alta aparece com resultado negativo (vermelho) no front.
**Correção esperada:** `valorAtual - valorInvestido`.
**Pega pelo teste:** `Resumo_CalculaResultadoPositivo` / `Resumo_CalculaResultadoNegativo`.

---

## D3 — Venda pode exceder a posição (regra removida)

**Arquivo:** `backend/src/PortfolioApi/Services/OrdemService.cs` → método `CriarAsync`

**Código CORRETO (base):**
```csharp
if (dto.Tipo == TipoOrdem.Venda && dto.Quantidade > ativo.Quantidade)
    throw new RegraNegocioException(
        $"Venda de {dto.Quantidade} excede a posição atual de {ativo.Quantidade} em {ativo.Ticker}.");
```

**Código COM DEFEITO (injetar):** **remover** o bloco `if` acima.

**Sintoma:** é possível vender mais do que se tem; a quantidade do ativo fica **negativa**.
**Correção esperada:** reintroduzir a validação (idealmente com um teste que a prove).
**Pega pelo teste:** `Venda_MaiorQuePosicao_LancaExcecao`.

---

## D4 — Percentual do resultado com denominador errado

**Arquivo:** `backend/src/PortfolioApi/Services/PortfolioService.cs`

**Código CORRETO (base):**
```csharp
var resultadoPercentualTotal = totalInvestido == 0
    ? 0m
    : Math.Round(resultadoTotal / totalInvestido * 100m, 2);
```

**Código COM DEFEITO (injetar):**
```csharp
// BUG: usa o valor atual como base do percentual em vez do investido
var resultadoPercentualTotal = totalAtual == 0
    ? 0m
    : Math.Round(resultadoTotal / totalAtual * 100m, 2);
```

**Sintoma:** o percentual de rentabilidade não bate com o esperado (ex.: 11,21% vira ~10%).
**Correção esperada:** usar `totalInvestido` como base.
**Observação:** ótimo para o participante **escrever um teste** que exponha a diferença.

---

## D5 — Cobertura de testes removida

**Pasta:** `backend/tests/PortfolioApi.Tests/`

**Ação:** remover (ou renomear para `.txt`) os arquivos de teste de regra de negócio,
mantendo apenas `TestDb.cs`:
- remover `OrdemServiceTests.cs`
- remover `PortfolioServiceTests.cs`
- remover `TickerValidatorTests.cs`

**Objetivo do Bloco B:** os participantes recriam a suíte com o `/tests` do Copilot,
incluindo os casos de borda — e, de quebra, os testes expõem D1–D4.

> 💡 Alternativa mais suave: manter os testes, mas **comentar** os `Assert` mais fortes,
> deixando testes "verdes porém inúteis" para o participante fortalecer.

---

## D6 — Validação de ticker duplicada (code smell)

**Arquivo:** `backend/src/PortfolioApi/Services/WatchlistService.cs` → método `ValidarEntrada`

**Código CORRETO (base):**
```csharp
if (!TickerValidator.IsValid(ticker))
    throw new RegraNegocioException($"Ticker inválido: '{ticker}'. Use o padrão da B3 (ex.: VALE3).");
```

**Código COM DEFEITO (injetar):** substituir a chamada ao validador por regex inline,
duplicando lógica que já existe em `TickerValidator`:
```csharp
// SMELL: valida o ticker "na mão", duplicando a regra que já existe em TickerValidator
var padrao = new System.Text.RegularExpressions.Regex(@"^[A-Z]{4}\d{1,2}$");
if (string.IsNullOrWhiteSpace(ticker) || !padrao.IsMatch(ticker.Trim().ToUpper()))
    throw new RegraNegocioException($"Ticker inválido: '{ticker}'. Use o padrão da B3 (ex.: VALE3).");
```

**Correção esperada:** substituir pela chamada a `TickerValidator.IsValid` (DRY).

---

## D7 — Lógica de negócio vazando para o controller

**Arquivo:** `backend/src/PortfolioApi/Controllers/OrdensController.cs`

**Código CORRETO (base):** o controller apenas delega ao service:
```csharp
[HttpPost]
public async Task<ActionResult<OrdemDto>> Criar(OrdemCreateDto dto)
{
    var criada = await _service.CriarAsync(dto);
    return CreatedAtAction(nameof(Obter), new { id = criada.Id }, criada);
}
```

**Código COM DEFEITO (injetar):** validação de negócio inline no controller
(quebra a separação de camadas):
```csharp
[HttpPost]
public async Task<ActionResult<OrdemDto>> Criar(OrdemCreateDto dto)
{
    // SMELL: regra de negócio no controller — deveria estar no OrdemService
    if (dto.Quantidade <= 0 || dto.Preco <= 0)
        return BadRequest(new { erro = "Valores inválidos." });

    var criada = await _service.CriarAsync(dto);
    return CreatedAtAction(nameof(Obter), new { id = criada.Id }, criada);
}
```

**Correção esperada:** mover/remover a regra do controller, mantendo-a no service.

---

## D8 — Nomes ruins e ausência de documentação

**Arquivo:** `backend/src/PortfolioApi/Services/OrdemService.cs` → método `AtualizarPosicao`

**Ação:** renomear o método e variáveis para nomes obscuros e **remover o XML doc**:
- `AtualizarPosicao` → `Proc`
- `custoAtual` → `x`, `custoNovo` → `y`, `novaQuantidade` → `n`
- apagar o comentário `/// <summary>...`

**Correção esperada:** usar o Copilot para **renomear** com clareza e **gerar docstrings**.

---

## 🔁 Como aplicar (sugestão de fluxo)

```bash
git switch -c feature/lab            # a partir da main limpa
# (Copilot injeta D1..D8 conforme este gabarito)
cd backend && dotnet build           # deve COMPILAR (defeitos são de lógica)
git add -A && git commit -m "lab: cenário com defeitos propositais para o workshop"
git push -u origin feature/lab
```

## ✅ Como o condutor valida a solução
- `dotnet test` **vermelho** na branch de exercícios (por D1–D4, após recriar testes em D5).
- Após as correções, `dotnet test` **verde** e o resumo da carteira batendo com o cálculo manual.
- Front exibe resultado positivo em **verde** e sem posições negativas.
