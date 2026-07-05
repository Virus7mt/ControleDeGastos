import { useEffect, useState } from 'react';
import { api } from '../api';
import { RelatorioTotais } from '../types';

// Componente responsável por mostrar o relatório de totais:
// receitas, despesas e saldo de cada pessoa + o total geral no final.
export default function ConsultaTotais() {
  const [relatorio, setRelatorio] = useState<RelatorioTotais | null>(null);
  const [erro, setErro] = useState('');

  useEffect(() => {
    carregar();
  }, []);

  async function carregar() {
    try {
      const dados = await api.consultarTotais();
      setRelatorio(dados);
    } catch (e) {
      setErro('Não foi possível carregar os totais. Verifique se a API está rodando.');
    }
  }

  return (
    <section>
      <h2>Consulta de Totais</h2>
      <button onClick={carregar}>Atualizar</button>

      {erro && <p className="erro">{erro}</p>}

      {relatorio && (
        <>
          <table>
            <thead>
              <tr>
                <th>Pessoa</th>
                <th>Total Receitas</th>
                <th>Total Despesas</th>
                <th>Saldo</th>
              </tr>
            </thead>
            <tbody>
              {relatorio.pessoas.map((p) => (
                <tr key={p.pessoaId}>
                  <td>{p.nome}</td>
                  <td>R$ {p.totalReceitas.toFixed(2)}</td>
                  <td>R$ {p.totalDespesas.toFixed(2)}</td>
                  <td className={p.saldo < 0 ? 'negativo' : 'positivo'}>
                    R$ {p.saldo.toFixed(2)}
                  </td>
                </tr>
              ))}
              {relatorio.pessoas.length === 0 && (
                <tr>
                  <td colSpan={4}>Nenhuma pessoa cadastrada ainda.</td>
                </tr>
              )}
            </tbody>
          </table>

          <h3>Total Geral</h3>
          <ul className="total-geral">
            <li>Receitas: R$ {relatorio.totalGeralReceitas.toFixed(2)}</li>
            <li>Despesas: R$ {relatorio.totalGeralDespesas.toFixed(2)}</li>
            <li>
              Saldo líquido:{' '}
              <strong className={relatorio.saldoGeral < 0 ? 'negativo' : 'positivo'}>
                R$ {relatorio.saldoGeral.toFixed(2)}
              </strong>
            </li>
          </ul>
        </>
      )}
    </section>
  );
}
