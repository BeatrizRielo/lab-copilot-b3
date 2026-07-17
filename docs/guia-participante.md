# 🎓 Guia do Participante — Lab GitHub Copilot (B3)

Bem-vindo(a)! Neste lab você vai usar o **GitHub Copilot** para evoluir uma aplicação
de **carteira de investimentos** (ações, FIIs e ETFs da B3). O objetivo **não** é escrever
muito código do zero, e sim praticar três superpoderes do Copilot:

> 🔍 **Revisar** · ✅ **Gerar testes** · 🔧 **Refatorar**

Você vai trabalhar sobre uma branch que contém **defeitos propositais**. Sua missão é
encontrá-los e corrigi-los com o apoio do Copilot.

---

## ⚙️ Setup (5 min)

1. Abra o repositório no **VS Code** (ou Codespaces).
2. Confirme que a extensão **GitHub Copilot** e o **Copilot Chat** estão ativos.
3. Faça checkout da branch de exercícios (o condutor vai informar o nome, ex.: `feature/lab`).
4. Suba o backend e o frontend (veja o [README](../README.md)).
5. Rode os testes: `cd backend && dotnet test`.

---

## 🧭 Como interagir com o Copilot

| Recurso | Onde | Para quê |
|---------|------|----------|
| **Copilot Chat** | painel lateral | perguntar, revisar, planejar |
| `/explain` | chat | entender um trecho selecionado |
| `/fix` | chat | sugerir correção de um problema |
| `/tests` | chat | gerar testes para o código selecionado |
| **Inline (Ctrl+I)** | editor | editar/gerar no ponto do cursor |
| **Comentário → código** | editor | escreva a intenção e deixe o Copilot completar |

> 💡 **Dica de ouro:** quanto melhor o contexto (arquivo aberto, código selecionado,
> prompt específico), melhor a sugestão. Seja explícito sobre a regra de negócio.

---

## 🧩 Bloco A — 🔍 Revisar código (40 min)

**Meta:** entender a base e encontrar bugs/melhorias com o Copilot.

1. Abra `backend/src/PortfolioApi/Services/OrdemService.cs`, selecione o método de
   atualização de posição e use **`/explain`**.
2. No Chat, peça:
   > *"Revise este método e aponte possíveis bugs de cálculo, casos não tratados e problemas de qualidade."*
3. Faça o mesmo com `PortfolioService.cs` (cálculo do resumo/P&L).
4. Rode a aplicação e compare o resultado do resumo com o que você calcularia na mão.

**Entregável:** liste os problemas encontrados e corrija **pelo menos 1 bug** de cálculo.

**Prompts sugeridos:**
- *"Este cálculo de preço médio está correto para uma operação de venda?"*
- *"Há algum problema de divisão por zero ou overflow aqui?"*
- *"Que casos de borda este endpoint não trata?"*

---

## 🧩 Bloco B — ✅ Gerar testes (40 min)

**Meta:** aumentar a cobertura das regras de negócio com o Copilot.

1. Abra `OrdemService.cs` e use **`/tests`** para gerar testes do cálculo de posição.
2. Peça **casos de borda** ao Copilot:
   > *"Gere testes para: venda maior que a posição, quantidade zero, preço negativo e ticker inválido."*
3. Coloque os testes em `backend/tests/PortfolioApi.Tests/` seguindo o padrão dos arquivos existentes
   (use `TestDb.CreateContext()`).
4. Rode `dotnet test` e garanta que ficam **verdes**.

**Entregável:** nova suíte de testes passando, cobrindo a regra crítica.

**Prompts sugeridos:**
- *"Gere testes xUnit no estilo Theory/InlineData para validação de ticker."*
- *"Escreva um teste que prove o bug que encontrei no Bloco A."* (teste vermelho → corrija → verde)

---

## 🧩 Bloco C — 🔧 Refatorar (50 min)

**Meta:** melhorar o design **sem quebrar** os testes (eles são sua rede de segurança).

Sugestões de refatoração guiada pelo Copilot:
1. **Remover duplicação** de validação de ticker espalhada pelos services.
2. **Extrair** lógica de negócio que porventura esteja no controller para o service.
3. **Renomear** variáveis pouco claras e **documentar** métodos públicos.
4. Após cada mudança, rode `dotnet test`.

**Entregável:** código mais limpo com **todos os testes ainda verdes**.

**Prompts sugeridos:**
- *"Esta validação está duplicada em 3 lugares. Como extrair para um único ponto?"*
- *"Refatore este método longo em funções menores e bem nomeadas."*
- *"Gere docstrings XML para os métodos públicos deste service."*

---

## 🧩 Bloco D — 🚀 Evoluir (opcional, 30 min)

**Meta:** adicionar uma funcionalidade nova conduzida por prompts.

Ideias:
- Filtro por **tipo de ativo** na listagem (`?tipo=FII`).
- Novo campo **setor** no ativo (com migração de dados e ajuste do front).
- Endpoint de **maior alta/baixa** do dia no resumo.

**Entregável:** feature nova com pelo menos 1 teste.

---

## ✅ Checklist final
- [ ] Encontrei e corrigi o(s) bug(s) de cálculo (Bloco A)
- [ ] Adicionei testes de borda e estão verdes (Bloco B)
- [ ] Refatorei removendo duplicação, testes seguem verdes (Bloco C)
- [ ] (Opcional) Implementei uma evolução (Bloco D)

---

## 🏆 Boas práticas de prompt aprendidas
- Dê **contexto de negócio** ("preço médio ponderado", "venda não pode exceder posição").
- Peça **casos de borda** explicitamente.
- Use testes como **rede de segurança** antes de refatorar.
- **Revise sempre** a sugestão do Copilot — você é o piloto, ele é o copiloto.
