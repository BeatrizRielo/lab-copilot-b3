# 0002 — SQLite + EF Core como persistência

- **Status:** Aceito
- **Data:** 2026-07-20
- **Decisores:** Time do Lab B3

## Contexto
O lab precisa de persistência realista, mas com **setup zero** para os participantes
(sem instalar/servir um banco externo) e com testes rápidos e isolados.

## Decisão
Usar **EF Core** com provedor **SQLite**. Nos testes, usar SQLite **in-memory**
via `TestDb.CreateContext()`, garantindo isolamento por teste.

## Alternativas Consideradas
- **SQL Server / PostgreSQL em container:** mais próximo de produção, mas adiciona
  fricção de setup (Docker, connection strings). Rejeitado para o lab.
- **EF Core InMemory provider:** simples, porém não valida constraints e SQL real.
  Rejeitado em favor do SQLite in-memory.

## Consequências
### Positivas
- Onboarding imediato; testes rápidos e determinísticos.
- SQL real (constraints, tipos) validado nos testes.

### Negativas / Trade-offs
- Algumas features específicas de bancos maiores não são exercitadas.
