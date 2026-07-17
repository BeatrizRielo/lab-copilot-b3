import { useEffect, useState } from 'react'
import { api } from '../api.js'

const vazio = { ticker: '', precoAlvo: '' }

export default function Watchlist() {
  const [itens, setItens] = useState([])
  const [form, setForm] = useState(vazio)
  const [erro, setErro] = useState('')

  async function carregar() {
    try {
      setItens(await api.listarWatchlist())
    } catch (e) {
      setErro(e.message)
    }
  }

  useEffect(() => {
    carregar()
  }, [])

  async function salvar(e) {
    e.preventDefault()
    setErro('')
    try {
      await api.criarWatchlist({
        ticker: form.ticker,
        precoAlvo: Number(form.precoAlvo),
      })
      setForm(vazio)
      carregar()
    } catch (e) {
      setErro(e.message)
    }
  }

  async function remover(id) {
    setErro('')
    try {
      await api.removerWatchlist(id)
      carregar()
    } catch (e) {
      setErro(e.message)
    }
  }

  return (
    <div className="card">
      <h2>Watchlist</h2>
      {erro && <div className="erro">{erro}</div>}

      <form onSubmit={salvar}>
        <label>
          Ticker
          <input value={form.ticker} onChange={(e) => setForm({ ...form, ticker: e.target.value })} placeholder="VALE3" />
        </label>
        <label>
          Preço-alvo
          <input type="number" step="0.01" value={form.precoAlvo} onChange={(e) => setForm({ ...form, precoAlvo: e.target.value })} />
        </label>
        <button className="primary" type="submit">Adicionar</button>
      </form>

      <table>
        <thead>
          <tr>
            <th>Ticker</th><th>Preço-alvo</th><th></th>
          </tr>
        </thead>
        <tbody>
          {itens.map((w) => (
            <tr key={w.id}>
              <td>{w.ticker}</td>
              <td>R$ {w.precoAlvo.toFixed(2)}</td>
              <td><button className="danger" onClick={() => remover(w.id)}>Remover</button></td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
