# Controle de Gastos Residenciais

Sistema simples para controle de gastos de uma residência, feito para um teste técnico.

- **Back-end:** .NET 8 (ASP.NET Core Web API) + Entity Framework Core + SQLite
- **Front-end:** React + TypeScript (Vite)

## Como o projeto está organizado

```
ControleGastos/
├── backend/
│   └── GastosResidenciais.Api/     -> API em C#
└── frontend/
    └── gastos-residenciais-web/    -> aplicação em React + TS
```

## Regras de negócio implementadas

- Pessoa tem: Id (gerado automaticamente), Nome e Idade.
- Ao deletar uma pessoa, todas as transações dela são apagadas automaticamente
  (configurado no `AppDbContext.cs` com `DeleteBehavior.Cascade`).
- Transação tem: Id (gerado automaticamente), Descrição, Valor, Tipo (Receita/Despesa)
  e o Id da pessoa dona da transação.
- O `PessoaId` informado precisa existir no cadastro (validado no `TransacoesController.cs`).
- Pessoas menores de 18 anos só podem ter **despesas** cadastradas, nunca receitas
  (também validado no `TransacoesController.cs` — essa é a validação que realmente vale,
  o front-end só esconde a opção na tela para ajudar visualmente).
- A consulta de totais mostra, para cada pessoa, o total de receitas, total de despesas
  e o saldo (receita - despesa), e no final soma tudo num total geral
  (feito no `TotaisController.cs`).

## Persistência dos dados

O back-end usa **SQLite**, que salva tudo em um arquivo (`gastos.db`) dentro da pasta
do projeto. Isso quer dizer que os dados continuam salvos mesmo depois de fechar a
API ou o navegador — é só abrir o sistema de novo que os cadastros continuam lá.

## Como rodar

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (versão 18 ou superior)

### 1. Rodando o back-end (API)

```bash
cd backend/GastosResidenciais.Api
dotnet restore
dotnet run
```

A API vai subir em algo como `http://localhost:5000` (ou outra porta parecida —
confira a mensagem que aparece no terminal ao rodar `dotnet run`). Se a porta for
diferente de 5000, ajuste a constante `API_URL` no arquivo
`frontend/gastos-residenciais-web/src/api.ts`.

Você pode testar os endpoints direto pelo Swagger, acessando `http://localhost:5000/swagger`.

### 2. Rodando o front-end

Em outro terminal:

```bash
cd frontend/gastos-residenciais-web
npm install
npm run dev
```

O front-end vai abrir em `http://localhost:5173`.

## Observações

- Como o teste pedia apenas criação e listagem de transações, não implementei
  edição/deleção de transações (só de pessoas, que também pede deleção).
- Usei `EnsureCreated()` no `Program.cs` em vez de Migrations do EF Core para
  facilitar rodar o projeto sem precisar instalar a ferramenta `dotnet-ef` à parte.
- Os comentários no código explicam a lógica de cada parte, principalmente onde
  ficam as regras de negócio (menor de idade, cascata de exclusão, cálculo de totais).
