using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Dtos
{
    // Dados que o front-end envia para CRIAR uma transação.
    public class CriarTransacaoDto
    {
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public TipoTransacao Tipo { get; set; }
        public int PessoaId { get; set; }
    }

    // Dados que a API devolve ao listar/criar uma transação.
    // Já incluo o nome da pessoa aqui para o front-end não precisar
    // fazer outra chamada só para mostrar "de quem" é a transação.
    public class TransacaoDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public TipoTransacao Tipo { get; set; }
        public int PessoaId { get; set; }
        public string PessoaNome { get; set; } = string.Empty;
    }
}
