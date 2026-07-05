import { useEffect, useState } from 'react';
import { api } from '../api';
import { Pessoa } from '../types';

// Componente responsável pelo cadastro de pessoas: criar, listar e deletar.
export default function CadastroPessoas() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [nome, setNome] = useState('');
  const [idade, setIdade] = useState('');
  const [erro, setErro] = useState('');
  const [carregando, setCarregando] = useState(false);

  // Busca a lista de pessoas assim que o componente é montado na tela.
  useEffect(() => {
    carregarPessoas();
  }, []);

  async function carregarPessoas() {
    try {
      const dados = await api.listarPessoas();
      setPessoas(dados);
    } catch (e) {
      setErro('Não foi possível carregar as pessoas. Verifique se a API está rodando.');
    }
  }

  async function handleCriar(e: React.FormEvent) {
    e.preventDefault();
    setErro('');

    // Validação simples no front-end antes de mandar para a API
    // (a validação "de verdade" continua sendo feita no back-end).
    if (!nome.trim()) {
      setErro('Informe o nome.');
      return;
    }
    const idadeNumero = Number(idade);
    if (Number.isNaN(idadeNumero) || idadeNumero < 0) {
      setErro('Informe uma idade válida.');
      return;
    }

    try {
      setCarregando(true);
      await api.criarPessoa(nome.trim(), idadeNumero);
      setNome('');
      setIdade('');
      await carregarPessoas();
    } catch (e: any) {
      setErro(e.message || 'Erro ao criar pessoa.');
    } finally {
      setCarregando(false);
    }
  }

  async function handleDeletar(id: number) {
    // Aviso extra aqui porque deletar a pessoa também apaga as transações dela.
    const confirmar = window.confirm(
      'Tem certeza? Todas as transações dessa pessoa também serão apagadas.'
    );
    if (!confirmar) return;

    try {
      await api.deletarPessoa(id);
      await carregarPessoas();
    } catch (e: any) {
      setErro(e.message || 'Erro ao deletar pessoa.');
    }
  }

  return (
    <section>
      <h2>Cadastro de Pessoas</h2>

      <form onSubmit={handleCriar} className="formulario">
        <input
          type="text"
          placeholder="Nome"
          value={nome}
          onChange={(e) => setNome(e.target.value)}
        />
        <input
          type="number"
          placeholder="Idade"
          value={idade}
          onChange={(e) => setIdade(e.target.value)}
          min={0}
        />
        <button type="submit" disabled={carregando}>
          Adicionar
        </button>
      </form>

      {erro && <p className="erro">{erro}</p>}

      <table>
        <thead>
          <tr>
            <th>Id</th>
            <th>Nome</th>
            <th>Idade</th>
            <th>Menor de idade?</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {pessoas.map((pessoa) => (
            <tr key={pessoa.id}>
              <td>{pessoa.id}</td>
              <td>{pessoa.nome}</td>
              <td>{pessoa.idade}</td>
              <td>{pessoa.ehMenorDeIdade ? 'Sim' : 'Não'}</td>
              <td>
                <button onClick={() => handleDeletar(pessoa.id)}>Excluir</button>
              </td>
            </tr>
          ))}
          {pessoas.length === 0 && (
            <tr>
              <td colSpan={5}>Nenhuma pessoa cadastrada ainda.</td>
            </tr>
          )}
        </tbody>
      </table>
    </section>
  );
}
