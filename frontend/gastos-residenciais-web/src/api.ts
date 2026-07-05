import { Pessoa, Transacao, RelatorioTotais, TipoTransacao } from './types';

// URL base da API do back-end em .NET.
// Se a sua API subir em outra porta, é só ajustar aqui.
const API_URL = 'http://localhost:5000/api';

// Função auxiliar para tratar as respostas de forma parecida em todas as chamadas:
// - Se der erro (status fora da faixa 2xx), joga uma exceção com a mensagem da API.
// - Se der certo, converte o corpo da resposta de JSON para objeto.
async function tratarResposta<T>(resposta: Response): Promise<T> {
  if (!resposta.ok) {
    const mensagem = await resposta.text();
    throw new Error(mensagem || 'Erro ao comunicar com a API.');
  }
  // Algumas respostas (como o DELETE) não têm corpo, por isso o cuidado extra aqui.
  const texto = await resposta.text();
  return texto ? (JSON.parse(texto) as T) : (undefined as T);
}

export const api = {
  // --- Pessoas ---

  listarPessoas: async (): Promise<Pessoa[]> => {
    const resposta = await fetch(`${API_URL}/pessoas`);
    return tratarResposta<Pessoa[]>(resposta);
  },

  criarPessoa: async (nome: string, idade: number): Promise<Pessoa> => {
    const resposta = await fetch(`${API_URL}/pessoas`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ nome, idade }),
    });
    return tratarResposta<Pessoa>(resposta);
  },

  deletarPessoa: async (id: number): Promise<void> => {
    const resposta = await fetch(`${API_URL}/pessoas/${id}`, {
      method: 'DELETE',
    });
    return tratarResposta<void>(resposta);
  },

  // --- Transações ---

  listarTransacoes: async (): Promise<Transacao[]> => {
    const resposta = await fetch(`${API_URL}/transacoes`);
    return tratarResposta<Transacao[]>(resposta);
  },

  criarTransacao: async (
    descricao: string,
    valor: number,
    tipo: TipoTransacao,
    pessoaId: number
  ): Promise<Transacao> => {
    const resposta = await fetch(`${API_URL}/transacoes`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ descricao, valor, tipo, pessoaId }),
    });
    return tratarResposta<Transacao>(resposta);
  },

  // --- Totais ---

  consultarTotais: async (): Promise<RelatorioTotais> => {
    const resposta = await fetch(`${API_URL}/totais`);
    return tratarResposta<RelatorioTotais>(resposta);
  },
};
