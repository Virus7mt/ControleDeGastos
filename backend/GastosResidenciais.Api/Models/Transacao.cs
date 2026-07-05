using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GastosResidenciais.Api.Models
{
    // Representa uma transação financeira (receita ou despesa) de uma pessoa.
    public class Transacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal Valor { get; set; }

        [Required]
        public TipoTransacao Tipo { get; set; }

        // Chave estrangeira: liga a transação à pessoa dona dela.
        [Required]
        public int PessoaId { get; set; }

        // Propriedade de navegação. É opcional (nullable) porque, na hora de CRIAR
        // uma transação, o front-end manda só o PessoaId, e não o objeto Pessoa inteiro.
        [ForeignKey(nameof(PessoaId))]
        public Pessoa? Pessoa { get; set; }
    }
}
