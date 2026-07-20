# Plano de Implementação — [NOME DA FEATURE]

**Branch:** `[###-nome-curto]` · **Spec:** [./spec.md](./spec.md)

## Verificação da Constituição (Gate)
> Confirme conformidade antes de planejar. Justifique qualquer exceção.

- [ ] Domínio em português (Artigo I)
- [ ] Regras em `Services/`, controllers só delegam (Artigo II)
- [ ] Regras de negócio críticas preservadas (Artigo III)
- [ ] Testes primeiro, com casos de borda (Artigo IV)
- [ ] Sem duplicação / over-engineering (Artigo V)

## 1. Contexto Técnico
- **Stack:** ASP.NET Core 9 Web API, EF Core (SQLite), xUnit · React + Vite.
- **Arquivos prováveis:** [liste caminhos que serão tocados]

## 2. Abordagem
[Descreva a estratégia técnica em alto nível — decisões e trade-offs.]

## 3. Modelo de Dados (se aplicável)
[Entidades, campos, relacionamentos, migrações necessárias.]

## 4. Contratos / Endpoints (se aplicável)
| Método | Rota | Entrada | Saída | Regra |
|--------|------|---------|-------|-------|
| | | | | |

## 5. Estratégia de Testes
[Quais testes provam a feature; casos de borda obrigatórios.]

## 6. Riscos
- [Risco → mitigação]

## 7. Decisões de Arquitetura (ADR)
- [ ] Alguma decisão arquitetural foi tomada? Se sim, registre em `docs/adr/` e referencie aqui.
- Relacionados: [ex.: docs/adr/0004-spec-driven-development.md]
