import { useState } from 'react'
import Ativos from './components/Ativos.jsx'
import Ordens from './components/Ordens.jsx'
import Watchlist from './components/Watchlist.jsx'
import Resumo from './components/Resumo.jsx'
import Rebalanceamento from './components/Rebalanceamento.jsx'

const ABAS = [
  { id: 'resumo', label: 'Resumo' },
  { id: 'ativos', label: 'Ativos' },
  { id: 'ordens', label: 'Ordens' },
  { id: 'watchlist', label: 'Watchlist' },
  { id: 'rebalanceamento', label: 'Rebalanceamento' },
]

export default function App() {
  const [aba, setAba] = useState('resumo')

  return (
    <>
      <header>
        <h1>Carteira B3</h1>
        <small>Lab GitHub Copilot — revisar, testar e refatorar</small>
      </header>
      <div className="container">
        <nav className="tabs">
          {ABAS.map((a) => (
            <button
              key={a.id}
              className={aba === a.id ? 'active' : ''}
              onClick={() => setAba(a.id)}
            >
              {a.label}
            </button>
          ))}
        </nav>

        {aba === 'resumo' && <Resumo />}
        {aba === 'ativos' && <Ativos />}
        {aba === 'ordens' && <Ordens />}
        {aba === 'watchlist' && <Watchlist />}
        {aba === 'rebalanceamento' && <Rebalanceamento />}
      </div>
    </>
  )
}
