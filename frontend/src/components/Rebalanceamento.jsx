import { useEffect, useState } from 'react'
import { api } from '../api.js'

function moeda(v) {
  return v.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
}

const SITUACAO = {
  Acima: { rotulo: 'Concentração excessiva', classe: 'situacao-acima' },
  Abaixo: { rotulo: 'Abaixo do alvo', classe: 'situacao-abaixo' },
  Equilibrada: { rotulo: 'Equilibrada', classe: 'situacao-ok' },
}

const ACAO = {
  Reduzir: { rotulo: 'Reduzir', classe: 'negativo' },
  Aumentar: { rotulo: 'Aumentar', classe: 'positivo' },
}

export default function Rebalanceamento() {
  const [dados, setDados] = useState(null)
  const [erro, setErro] = useState('')

  useEffect(() => {
    api.rebalanceamento().then(setDados).catch((e) => setErro(e.message))
  }, [])

  if (erro) return <div className="erro">{erro}</div>
  if (!dados) return <div className="card">Carregando…</div>

  return (
    <div className="card">
      <h2>Rebalanceamento da carteira</h2>

      <table>
        <thead>
          <tr>
            <th>Classe</th><th>Atual</th><th>Alvo</th><th>Situação</th>
          </tr>
        </thead>
        <tbody>
          {dados.alocacoes.map((a) => {
            const s = SITUACAO[a.situacao] ?? { rotulo: a.situacao, classe: '' }
            return (
              <tr key={a.classe}>
                <td>{a.classe}</td>
                <td>{a.percentualAtual.toFixed(2)}%</td>
                <td>{a.percentualAlvo.toFixed(2)}%</td>
                <td><span className={`tag ${s.classe}`}>{s.rotulo}</span></td>
              </tr>
            )
          })}
        </tbody>
      </table>

      <h3>Sugestões de ajuste</h3>
      {dados.sugestoes.length === 0 ? (
        <p className="vazio">Carteira equilibrada — nenhum ajuste necessário.</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Ticker</th><th>Ação</th><th>Quantidade</th>
            </tr>
          </thead>
          <tbody>
            {dados.sugestoes.map((s, i) => {
              const acao = ACAO[s.acao] ?? { rotulo: s.acao, classe: '' }
              return (
                <tr key={`${s.ticker}-${i}`}>
                  <td>{s.ticker}</td>
                  <td className={acao.classe}>{acao.rotulo}</td>
                  <td>{s.quantidade}</td>
                </tr>
              )
            })}
          </tbody>
        </table>
      )}

      <div className="resumo-cards">
        <div className="box">
          <span>Corretagem</span>
          <strong>{moeda(dados.custo.corretagemTotal)}</strong>
        </div>
        <div className="box">
          <span>IR estimado</span>
          <strong>{moeda(dados.custo.irEstimado)}</strong>
        </div>
        <div className="box">
          <span>Custo total</span>
          <strong>{moeda(dados.custo.custoTotal)}</strong>
        </div>
      </div>
    </div>
  )
}
