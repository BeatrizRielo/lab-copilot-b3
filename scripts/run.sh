#!/usr/bin/env bash
# Executa backend (ASP.NET Core) e frontend (Vite) da carteira B3.
# Uso:  ./scripts/run.sh
#
# Backend:  http://localhost:5000  (Swagger em /swagger)
# Frontend: http://localhost:5173
# Encerra ambos com Ctrl+C.

set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
BACKEND="$ROOT/backend/src/PortfolioApi"
FRONTEND="$ROOT/frontend"

for cmd in dotnet npm; do
  if ! command -v "$cmd" >/dev/null 2>&1; then
    echo "'$cmd' não encontrado no PATH. Instale-o antes de continuar." >&2
    exit 1
  fi
done

if [ ! -d "$FRONTEND/node_modules" ]; then
  echo "Instalando dependências do frontend..."
  (cd "$FRONTEND" && npm install)
fi

pids=()
cleanup() {
  echo ""
  echo "Encerrando serviços..."
  kill "${pids[@]}" 2>/dev/null || true
}
trap cleanup EXIT INT TERM

echo "Iniciando backend (http://localhost:5000)..."
(cd "$BACKEND" && dotnet run) &
pids+=($!)

echo "Iniciando frontend (http://localhost:5173)..."
(cd "$FRONTEND" && npm run dev) &
pids+=($!)

echo ""
echo "Aplicação iniciada. Pressione Ctrl+C para encerrar."
wait
