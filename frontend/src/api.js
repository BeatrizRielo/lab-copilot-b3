// Cliente HTTP simples para a API de carteira.
const BASE = '/api'

async function handle(res) {
  if (!res.ok) {
    let msg = `Erro ${res.status}`
    try {
      const body = await res.json()
      if (body?.erro) msg = body.erro
    } catch {
      // resposta sem corpo JSON
    }
    throw new Error(msg)
  }
  if (res.status === 204) return null
  return res.json()
}

export const api = {
  // Ativos
  listarAtivos: () => fetch(`${BASE}/ativos`).then(handle),
  criarAtivo: (dto) => fetch(`${BASE}/ativos`, post(dto)).then(handle),
  atualizarAtivo: (id, dto) => fetch(`${BASE}/ativos/${id}`, put(dto)).then(handle),
  removerAtivo: (id) => fetch(`${BASE}/ativos/${id}`, del()).then(handle),

  // Ordens
  listarOrdens: () => fetch(`${BASE}/ordens`).then(handle),
  criarOrdem: (dto) => fetch(`${BASE}/ordens`, post(dto)).then(handle),
  removerOrdem: (id) => fetch(`${BASE}/ordens/${id}`, del()).then(handle),

  // Watchlist
  listarWatchlist: () => fetch(`${BASE}/watchlist`).then(handle),
  criarWatchlist: (dto) => fetch(`${BASE}/watchlist`, post(dto)).then(handle),
  atualizarWatchlist: (id, dto) => fetch(`${BASE}/watchlist/${id}`, put(dto)).then(handle),
  removerWatchlist: (id) => fetch(`${BASE}/watchlist/${id}`, del()).then(handle),

  // Resumo
  resumo: () => fetch(`${BASE}/portfolio/resumo`).then(handle),
}

function post(body) {
  return { method: 'POST', headers: json(), body: JSON.stringify(body) }
}
function put(body) {
  return { method: 'PUT', headers: json(), body: JSON.stringify(body) }
}
function del() {
  return { method: 'DELETE' }
}
function json() {
  return { 'Content-Type': 'application/json' }
}
