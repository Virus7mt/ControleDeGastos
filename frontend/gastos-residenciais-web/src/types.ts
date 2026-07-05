// Tipos usados em toda a aplicação, espelhando os DTOs do back-end em C#.
// Manter isso sincronizado com o back-end ajuda a evitar bugs bobos de "campo errado".

export type TipoTransacao = 'Receita' | 'Despesa';

export interface Pessoa {
  id: number;
  nome: string;
  idade: number;
  ehMenorDeIdade: boolean;
}

export interface Transacao {
  id: number;
  descricao: string;
  valor: number;
  tipo: TipoTransacao;
  pessoaId: number;
  pessoaNome: string;
}

export interface TotalPorPessoa {
  pessoaId: number;
  nome: string;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

export interface RelatorioTotais {
  pessoas: TotalPorPessoa[];
  totalGeralReceitas: number;
  totalGeralDespesas: number;
  saldoGeral: number;
}
