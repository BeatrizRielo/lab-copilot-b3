import { useEffect, useState } from 'react'
import { api } from '../api.js'

const vazio = { ativoId: '', tipo: 'Compra', quantidade: '', preco: '' }

export default function Ordens() {
  const [ordens, setOrdens] = useState([])
  const [ativos, setAtivos] = useState([])
  const [form, setForm] = useState(vazio)
  const [erro, setErro] = useState('')

  async function carregar() {
    try {
      const [o, a] = await Promise.all([api.listarOrdens(), api.listarAtivos()])
      setOrdens(o)
      setAtivos(a)
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
      await api.criarOrdem({
        ativoId: Number(form.ativoId),
        tipo: form.tipo,
        quantidade: Number(form.quantidade),
        preco: Number(form.preco),
        data: null,
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
      await api.removerOrdem(id)
      carregar()
    } catch (e) {
      setErro(e.message)
    }
  }

  return (
    <div className="card">
      <h2>Ordens</h2>
      {erro && <div className="erro">{erro}</div>}

      <form onSubmit={salvar}>
        <label>
          Ativo
          <select value={form.ativoId} onChange={(e) => setForm({ ...form, ativoId: e.target.value })}>
            <option value="">Selecione</option>
            {ativos.map((a) => (
              <option key={a.id} value={a.id}>{a.ticker}</option>
            ))}
          </select>
        </label>
        <label>
          Tipo
          <select value={form.tipo} onChange={(e) => setForm({ ...form, tipo: e.target.value })}>
            <option value="Compra">Compra</option>
            <option value="Venda">Venda</option>
          </select>
        </label>
        <label>
          Quantidade
          <input type="number" value={form.quantidade} onChange={(e) => setForm({ ...form, quantidade: e.target.value })} />
        </label>
        <label>
          Preço
          <input type="number" step="0.01" value={form.preco} onChange={(e) => setForm({ ...form, preco: e.target.value })} />
        </label>
        <button className="primary" type="submit">Registrar</button>
      </form>

      <table>
        <thead>
          <tr>
            <th>Data</th><th>Ticker</th><th>Tipo</th><th>Qtd</th><th>Preço</th><th></th>
          </tr>
        </thead>
        <tbody>
          {ordens.map((o) => (
            <tr key={o.id}>
              <td>{new Date(o.data).toLocaleDateString('pt-BR')}</td>
              <td>{o.ticker}</td>
              <td>{o.tipo}</td>
              <td>{o.quantidade}</td>
              <td>R$ {o.preco.toFixed(2)}</td>
              <td><button className="danger" onClick={() => remover(o.id)}>Remover</button></td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
