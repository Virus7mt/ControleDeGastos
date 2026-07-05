using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GastosResidenciais.Api.Models
{
    // Representa uma pessoa cadastrada no sistema.
    // Essa classe é "espelhada" pelo Entity Framework Core em uma tabela do banco.
    public class Pessoa
    {
        // Identificador único, gerado automaticamente pelo banco (autoincrement).
        // Não precisamos setar isso na mão em nenhum momento.
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public int Idade { get; set; }

        // Propriedade de navegação: representa todas as transações dessa pessoa.
        // O Entity Framework usa isso para entender o relacionamento 1 (Pessoa) -> N (Transações).
        public List<Transacao> Transacoes { get; set; } = new();

        // Propriedade calculada (não é salva no banco, por isso o [NotMapped]).
        // Centraliza em um único lugar a regra "o que é ser menor de idade",
        // assim não fico repetindo "Idade < 18" espalhado pelo código.
        [NotMapped]
        public bool EhMenorDeIdade => Idade < 18;
    }
}
