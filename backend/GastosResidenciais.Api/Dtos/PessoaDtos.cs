namespace GastosResidenciais.Api.Dtos
{
    // DTO = Data Transfer Object.
    // Uso DTOs em vez de mandar a entidade "Pessoa" direto pela API por dois motivos:
    // 1) Evita expor detalhes internos do banco que não interessam ao front-end.
    // 2) Evita problemas de referência circular (Pessoa -> Transacoes -> Pessoa -> ...).

    // Dados que o front-end envia para CRIAR uma pessoa.
    // Note que não tem "Id" aqui: o Id é gerado automaticamente pelo banco.
    public class CriarPessoaDto
    {
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
    }

    // Dados que a API devolve para o front-end ao listar/criar uma pessoa.
    public class PessoaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
        public bool EhMenorDeIdade { get; set; }
    }
}
