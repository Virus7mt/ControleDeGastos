using Microsoft.EntityFrameworkCore;
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Data
{
    // O DbContext é a "ponte" entre as classes C# (Pessoa, Transacao) e o banco de dados.
    // É por ele que o Entity Framework Core sabe o que precisa criar/consultar no banco.
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pessoa> Pessoas => Set<Pessoa>();
        public DbSet<Transacao> Transacoes => Set<Transacao>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura o relacionamento entre Pessoa e Transacao.
            //
            // REGRA DE NEGÓCIO IMPORTANTE:
            // "Em casos que se delete uma pessoa, todas as transações dessa pessoa
            //  deverão ser apagadas."
            //
            // Isso é feito aqui com o DeleteBehavior.Cascade: quando uma Pessoa é
            // removida, o próprio banco de dados apaga automaticamente todas as
            // Transacoes que apontam para ela (não precisamos apagar uma por uma no código).
            modelBuilder.Entity<Pessoa>()
                .HasMany(p => p.Transacoes)
                .WithOne(t => t.Pessoa)
                .HasForeignKey(t => t.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Guarda o enum Tipo como texto ("Receita" / "Despesa") no banco,
            // em vez de número (0 / 1). Fica bem mais fácil de ler se eu abrir o banco
            // com alguma ferramenta para conferir os dados.
            modelBuilder.Entity<Transacao>()
                .Property(t => t.Tipo)
                .HasConversion<string>();
        }
    }
}
