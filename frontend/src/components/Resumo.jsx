import { useEffect, useState } from 'react'
import { api } from '../api.js'

function moeda(v) {
  return v.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
}

function Resultado({ valor, percentual }) {
  const classe = valor >= 0 ? 'positivo' : 'negativo'
  return (
    <span className={classe}>
      {moeda(valor)} ({percentual.toFixed(2)}%)
    </span>
  )
}

export default function Resumo() {
  const [dados, setDados] = useState(null)
  const [erro, setErro] = useState('')

  useEffect(() => {
    api.resumo().then(setDados).catch((e) => setErro(e.message))
  }, [])

  if (erro) return <div className="erro">{erro}</div>
  if (!dados) return <div className="card">Carregando…</div>

  return (
    <div className="card">
      <h2>Resumo da carteira</h2>

      <div className="resumo-cards">
        <div className="box">
          <span>Total investido</span>
          <strong>{moeda(dados.totalInvestido)}</strong>
        </div>
        <div className="box">
          <span>Valor atual</span>
          <strong>{moeda(dados.totalAtual)}</strong>
        </div>
        <div className="box">
          <span>Resultado (P&amp;L)</span>
          <strong>
            <Resultado valor={dados.resultadoTotal} percentual={dados.resultadoPercentualTotal} />
          </strong>
        </div>
      </div>

      <table>
        <thead>
          <tr>
            <th>Ticker</th><th>Qtd</th><th>Preço médio</th><th>Preço atual</th>
            <th>Investido</th><th>Atual</th><th>Resultado</th>
          </tr>
        </thead>
        <tbody>
          {dados.posicoes.map((p) => (
            <tr key={p.ticker}>
              <td>{p.ticker}</td>
              <td>{p.quantidade}</td>
              <td>{moeda(p.precoMedio)}</td>
              <td>{moeda(p.precoAtual)}</td>
              <td>{moeda(p.valorInvestido)}</td>
              <td>{moeda(p.valorAtual)}</td>
              <td><Resultado valor={p.resultado} percentual={p.resultadoPercentual} /></td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
