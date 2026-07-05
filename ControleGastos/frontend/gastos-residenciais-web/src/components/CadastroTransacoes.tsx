import { useEffect, useState } from 'react';
import { api } from '../api';
import { Pessoa, Transacao, TipoTransacao } from '../types';

// Componente responsável pelo cadastro de transações: criar e listar.
export default function CadastroTransacoes() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);

  const [descricao, setDescricao] = useState('');
  const [valor, setValor] = useState('');
  const [tipo, setTipo] = useState<TipoTransacao>('Despesa');
  const [pessoaId, setPessoaId] = useState('');
  const [erro, setErro] = useState('');
  const [carregando, setCarregando] = useState(false);

  useEffect(() => {
    carregarDados();
  }, []);

  async function carregarDados() {
    try {
      const [pessoasDados, transacoesDados] = await Promise.all([
        api.listarPessoas(),
        api.listarTransacoes(),
      ]);
      setPessoas(pessoasDados);
      setTransacoes(transacoesDados);
    } catch (e) {
      setErro('Não foi possível carregar os dados. Verifique se a API está rodando.');
    }
  }

  // Descobre se a pessoa selecionada no formulário é menor de idade.
  // Uso isso para já avisar o usuário e esconder a opção "Receita" na tela
  // — mas a regra de verdade quem garante é o back-end (nunca confio só no front-end).
  const pessoaSelecionada = pessoas.find((p) => p.id === Number(pessoaId));
  const pessoaEhMenor = pessoaSelecionada?.ehMenorDeIdade ?? false;

  async function handleCriar(e: React.FormEvent) {
    e.preventDefault();
    setErro('');

    if (!descricao.trim()) {
      setErro('Informe a descrição.');
      return;
    }
    const valorNumero = Number(valor);
    if (Number.isNaN(valorNumero) || valorNumero <= 0) {
      setErro('Informe um valor válido, maior que zero.');
      return;
    }
    if (!pessoaId) {
      setErro('Selecione uma pessoa.');
      return;
    }

    try {
      setCarregando(true);
      await api.criarTransacao(descricao.trim(), valorNumero, tipo, Number(pessoaId));
      setDescricao('');
      setValor('');
      await carregarDados();
    } catch (e: any) {
      // A API devolve uma mensagem de erro de negócio (ex: "menor de idade não
      // pode ter receita"), então eu mostro ela direto para o usuário.
      setErro(e.message || 'Erro ao criar transação.');
    } finally {
      setCarregando(false);
    }
  }

  return (
    <section>
      <h2>Cadastro de Transações</h2>

      <form onSubmit={handleCriar} className="formulario">
        <input
          type="text"
          placeholder="Descrição"
          value={descricao}
          onChange={(e) => setDescricao(e.target.value)}
        />
        <input
          type="number"
          placeholder="Valor"
          value={valor}
          onChange={(e) => setValor(e.target.value)}
          min={0}
          step="0.01"
        />
        <select value={tipo} onChange={(e) => setTipo(e.target.value as TipoTransacao)}>
          <option value="Despesa">Despesa</option>
          {/* Se a pessoa selecionada for menor de idade, nem mostro a opção
              de Receita — deixa a interface mais clara para quem estiver usando. */}
          {!pessoaEhMenor && <option value="Receita">Receita</option>}
        </select>
        <select value={pessoaId} onChange={(e) => setPessoaId(e.target.value)}>
          <option value="">Selecione a pessoa</option>
          {pessoas.map((p) => (
            <option key={p.id} value={p.id}>
              {p.nome} {p.ehMenorDeIdade ? '(menor de idade)' : ''}
            </option>
          ))}
        </select>
        <button type="submit" disabled={carregando}>
          Adicionar
        </button>
      </form>

      {pessoaEhMenor && (
        <p className="aviso">
          Essa pessoa é menor de idade: só é possível cadastrar despesas para ela.
        </p>
      )}
      {erro && <p className="erro">{erro}</p>}

      <table>
        <thead>
          <tr>
            <th>Id</th>
            <th>Descrição</th>
            <th>Valor</th>
            <th>Tipo</th>
            <th>Pessoa</th>
          </tr>
        </thead>
        <tbody>
          {transacoes.map((t) => (
            <tr key={t.id}>
              <td>{t.id}</td>
              <td>{t.descricao}</td>
              <td>R$ {t.valor.toFixed(2)}</td>
              <td>{t.tipo}</td>
              <td>{t.pessoaNome}</td>
            </tr>
          ))}
          {transacoes.length === 0 && (
            <tr>
              <td colSpan={5}>Nenhuma transação cadastrada ainda.</td>
            </tr>
          )}
        </tbody>
      </table>
    </section>
  );
}
