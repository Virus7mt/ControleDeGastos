import { useState } from 'react';
import CadastroPessoas from './components/CadastroPessoas';
import CadastroTransacoes from './components/CadastroTransacoes';
import ConsultaTotais from './components/ConsultaTotais';

// Componente principal da aplicação.
// Aqui eu só controlo qual "aba" está sendo exibida (bem simples, sem
// biblioteca de rotas, já que o sistema é pequeno).
type Aba = 'pessoas' | 'transacoes' | 'totais';

function App() {
  const [abaAtiva, setAbaAtiva] = useState<Aba>('pessoas');

  return (
    <div className="container">
      <h1>💰 Controle de Gastos Residenciais</h1>

      <nav className="menu">
        <button
          className={abaAtiva === 'pessoas' ? 'ativo' : ''}
          onClick={() => setAbaAtiva('pessoas')}
        >
          Pessoas
        </button>
        <button
          className={abaAtiva === 'transacoes' ? 'ativo' : ''}
          onClick={() => setAbaAtiva('transacoes')}
        >
          Transações
        </button>
        <button
          className={abaAtiva === 'totais' ? 'ativo' : ''}
          onClick={() => setAbaAtiva('totais')}
        >
          Totais
        </button>
      </nav>

      <main>
        {abaAtiva === 'pessoas' && <CadastroPessoas />}
        {abaAtiva === 'transacoes' && <CadastroTransacoes />}
        {abaAtiva === 'totais' && <ConsultaTotais />}
      </main>
    </div>
  );
}

export default App;
