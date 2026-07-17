import { useEffect, useState } from 'react'
import { api } from '../api.js'

const TIPOS = ['Acao', 'FII', 'ETF']
const vazio = { ticker: '', tipo: 'Acao', quantidade: '', precoMedio: '' }

export default function Ativos() {
  const [ativos, setAtivos] = useState([])
  const [form, setForm] = useState(vazio)
  const [erro, setErro] = useState('')

  async function carregar() {
    try {
      setAtivos(await api.listarAtivos())
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
      await api.criarAtivo({
        ticker: form.ticker,
        tipo: form.tipo,
        quantidade: Number(form.quantidade),
        precoMedio: Number(form.precoMedio),
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
      await api.removerAtivo(id)
      carregar()
    } catch (e) {
      setErro(e.message)
    }
  }

  return (
    <div className="card">
      <h2>Ativos</h2>
      {erro && <div className="erro">{erro}</div>}

      <form onSubmit={salvar}>
        <label>
          Ticker
          <input
            value={form.ticker}
            onChange={(e) => setForm({ ...form, ticker: e.target.value })}
            placeholder="PETR4"
          />
        </label>
        <label>
          Tipo
          <select value={form.tipo} onChange={(e) => setForm({ ...form, tipo: e.target.value })}>
            {TIPOS.map((t) => (
              <option key={t} value={t}>{t}</option>
            ))}
          </select>
        </label>
        <label>
          Quantidade
          <input
            type="number"
            value={form.quantidade}
            onChange={(e) => setForm({ ...form, quantidade: e.target.value })}
          />
        </label>
        <label>
          Preço médio
          <input
            type="number"
            step="0.01"
            value={form.precoMedio}
            onChange={(e) => setForm({ ...form, precoMedio: e.target.value })}
          />
        </label>
        <button className="primary" type="submit">Adicionar</button>
      </form>

      <table>
        <thead>
          <tr>
            <th>Ticker</th><th>Tipo</th><th>Qtd</th><th>Preço médio</th><th></th>
          </tr>
        </thead>
        <tbody>
          {ativos.map((a) => (
            <tr key={a.id}>
              <td>{a.ticker}</td>
              <td>{a.tipo}</td>
              <td>{a.quantidade}</td>
              <td>R$ {a.precoMedio.toFixed(2)}</td>
              <td><button className="danger" onClick={() => remover(a.id)}>Remover</button></td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
