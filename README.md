# 💼 Carteira B3 — Lab GitHub Copilot

Aplicação de exemplo para o **workshop de GitHub Copilot da B3**. Uma carteira de
investimentos simples (ações, FIIs e ETFs) usada para praticar as capacidades do
Copilot: **revisar código, gerar testes e refatorar**.

> Esta é a branch **base/solução** — código limpo, testes verdes e app funcionando.
> Durante o workshop, os participantes trabalham sobre uma branch com inclusão de
> uma nova capacidade na aplicação, realizada por um Dev durante uma sprint. Essas
> tasks estão em uma feature branch. O participante deve criar sua própria feature
> branch com origem na branch com alterações, avaliar o que foi modificado e criado,
> realizar o code review e commitar. Em seguida, no portal ou na IDE, deve criar um
> Pull Request para `main` com review do Copilot, validar e verificar as considerações
> do agent antes do merge (ver # 🎓 Guia do Participante — Lab GitHub Copilot (B3) 
> no final desse documento).

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

## 📌 Participantes - Add nome completo
Ana Beatriz Costa


# 🎓 Guia do Participante — Lab GitHub Copilot (B3)

Bem-vindo(a)! Neste lab você vai usar o **GitHub Copilot** para evoluir uma aplicação
de **carteira de investimentos** (ações, FIIs e ETFs da B3). O objetivo **não** é escrever código, e sim praticar três capacidades do Copilot no fluxo de revisão e Pull Request:

> 🔍 **Code review local** (Copilot Chat) · 🔀 **Copilot code review no PR pela IDE** · 🌐 **Copilot code review no PR pelo github.com**

Você vai trabalhar sobre uma branch que **já contém a implementação de uma feature** (Sprint 25 — sugestões de rebalanceamento). Sua missão é **revisá-la com o Copilot** e, se estiver de acordo, **abrir o Pull Request** — na IDE ou no portal — acionando o **Copilot code review**.

---

## ⚙️ Setup (5 min)

1. Abra o repositório no **VS Code** (ou Codespaces).
2. Confirme que a extensão **GitHub Copilot** e o **Copilot Chat** estão ativos.
3. Atualize a `main`:
   ```bash
   git checkout main
   ```
4. **Crie sua feature branch** a partir da branch já implementada (troque `NOME` pelo seu nome/usuário):
   ```bash
   git checkout -b feature/sprint-25-task-NOME origin/feature/sprint-25-task
   ```
5. (OPCIONAL) Suba o backend e o frontend com o script:
    ```bash
   bash ./scripts/run.sh
   ``` 
   ```powershell
   ./scripts/run.ps1
   ```

> ✅ A feature **já está implementada** nesta branch. Você **não vai escrever a feature** —
> Voce vai **revisá-la** com o Copilot (Bloco A) e depois **abrir o PR** (Bloco B).

> 💡 **Dica de ouro:** quanto melhor o contexto (arquivo aberto, código selecionado,
> prompt específico), melhor a sugestão. Seja explícito sobre a regra de negócio.

---

## 🧩 Bloco A — 🔍 Revisar suas alterações localmente (10 min)

**Meta:** revisar, **antes de abrir o PR**, as alterações que você fez na task usando as
capacidades do GitHub Copilot. Aqui você é o primeiro revisor do seu próprio código.

### Opções para revisar localmente com o Copilot

| Opção | Onde | Como usar |
|-------|------|-----------|
| **Revisar o diff no Chat** | Copilot Chat | use `#changes` para dar o diff como contexto e peça uma revisão. |
| **`/explain` em um trecho** | editor + Chat | selecione o método alterado e entenda o impacto da mudança. |
| **`/fix` pontual** | editor + Chat | proponha correção de um problema apontado. |
| **Inline Chat (Ctrl+I)** | editor | ajustes locais (nomes, mensagens, clareza) sem sair do arquivo. |

#### 💬 Prompts de code review para o Chat

Copie e cole no Copilot Chat usando `#changes` (suas alterações como contexto):

- **Code review geral**
  > *"#changes Faça um code review das minhas alterações. Liste os achados em crítico,
  > médio e melhoria, citando arquivo e linha."*

- **Revisar um arquivo específico**
  > *"#file:RebalanceamentoService.cs Faça o code review deste arquivo focando em
  > corretude do cálculo e clareza."*

- **Code review contra as especificações (SDD)**
  > *"#changes #file:spec.md Revise minhas alterações verificando se atendem aos
  > requisitos (RF/RN) e critérios de aceite da especificação. Aponte o que está
  > divergente ou faltando."*

### Passo a passo

1. **Veja o que mudou:** abra o painel **Source Control** e inspecione o diff de cada
   arquivo da sua task (ex.: `RebalanceamentoService.cs`, DTOs, `PortfolioController.cs`).

2. **Peça uma revisão contextual no Copilot Chat** usando suas mudanças como contexto:
   > *"#changes Faça um code review das minhas alterações com foco em regra de negócio,
   > bugs de cálculo, casos de borda e legibilidade. Classifique em: crítico, médio e melhoria."*

3. **Aprofunde nos trechos sensíveis** com `/explain` (ex.: cálculo de percentual por
   classe, custo de rebalanceamento) e confirme se a lógica bate com a regra do domínio.

4. **Aplique correções com segurança:** use `/fix` para propostas pontuais e
   **Inline (Ctrl+I)** para ajustes locais. Aceite apenas o que **preserva a regra de negócio**.

5. **Revalide as evidências:**
   ```bash
   dotnet test
   ```

6. **Registre sua participação e faça o commit:** adicione seu nome completo no final do
   [README.md](../README.md) em `## 📌 Participantes`. Essa é a única alteração real que você
   fará — assim o `#changes` terá conteúdo para revisar.

   > 💡 Peça ao Copilot Chat a mensagem de commit:
   > *"Gere uma mensagem de commit no padrão Conventional Commits para estas mudanças staged."*

   ```bash
   git add .
   git commit -m "docs: adiciona <seu nome> em participantes"
   git push -u origin feature/sprint-25-task-NOME
   ```

**Entregável:** feature revisada com o Copilot, testes verdes e branch publicada, pronta para o PR (Bloco B).

**Prompts sugeridos:**
- *"Há divisão por zero ou overflow no cálculo de percentual por classe?"*
- *"Que casos de borda esta sugestão de rebalanceamento não trata?"*
- *"Esta venda sugerida pode exceder a posição atual do ativo?"*

---

## 🧩 Bloco B — 🔀 Abrir o Pull Request (10 min)

**Meta:** publicar suas mudanças e abrir o PR, escolhendo entre a **IDE (VS Code)** ou o
**portal github.com**. Depois, acionar o **Copilot code review** no PR.

### Opção 1 — Abrir o PR pela IDE (VS Code)

Requer a extensão **GitHub Pull Requests**.

1. Confirme que sua branch foi enviada: `git push -u origin feature/sprint-25-task-NOME`.
2. Na aba **GitHub** (ícone do GitHub na Activity Bar) → **Create Pull Request**.
3. Defina **base = `main`** e **compare = `feature/sprint-25-task-NOME`**.
4. Gere título e descrição com o Copilot:
   > *"#changes Gere um resumo de PR com: contexto, o que foi implementado, testes
   > adicionados, riscos e como validar."*
5. Clique em **Create** (ou **Create as Draft** se ainda estiver em progresso).

### Opção 2 — Abrir o PR pelo portal github.com

1. Acesse o repositório no navegador. Um banner **"Compare & pull request"** aparece
   após o push — clique nele. (Ou vá em **Pull requests → New pull request**.)
2. Selecione **base: `main`** ← **compare: `feature/sprint-25-task-NOME`**.
3. Preencha **título** (ex.: `feat: sugestões de rebalanceamento (Sprint 25)`) e a
   **descrição** (cole o resumo gerado pelo Copilot).
4. Clique em **Create pull request** (use **Create draft pull request** se aplicável).

### Copilot code review no PR

1. No PR, abra o menu **Reviewers** e selecione **Copilot** para solicitar uma revisão automática.
2. O Copilot analisa o diff e publica **comentários linha a linha**, muitas vezes com
   **sugestões de correção prontas** (bloco *"suggested change"*).
3. Para cada sugestão:
   - **Commit suggestion** aplica o patch direto no PR; ou
   - traga o comentário para o Copilot Chat local:
     > *"Com base neste comentário de PR, proponha uma resposta técnica curta + patch sugerido + teste de validação."*
4. Aplique o ajuste, rode `dotnet test` novamente e **responda no PR com a evidência**.
5. Solicite também um **review humano** (colega/condutor) para fechar o ciclo.

**Entregável:** PR aberto, com Copilot code review acionado e evidências de qualidade (testes verdes).

---

## ✅ Checklist final
- [ ] Criei a branch `feature/sprint-25-task-NOME` a partir da `feature/sprint-25-task`
- [ ] Fiz commits descritivos e `push` da branch
- [ ] Revisei minhas alterações localmente com o Copilot (Bloco A)
- [ ] Abri o PR pela IDE ou pelo github.com (Bloco B)
- [ ] Acionei o **Copilot code review** e tratei as sugestões

---

## 🏆 Boas práticas de prompt aprendidas
- Dê **contexto de negócio** ("preço médio ponderado", "venda não pode exceder posição").
- Use `#changes` para revisar exatamente **o que você alterou**.
- Peça **casos de borda** explicitamente.
- Use testes como **rede de segurança** antes de refatorar.
- **Revise sempre** a sugestão do Copilot — você é o piloto, ele é o copiloto.
