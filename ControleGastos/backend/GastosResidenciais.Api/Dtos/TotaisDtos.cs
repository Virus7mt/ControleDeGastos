namespace GastosResidenciais.Api.Dtos
{
    // Totais (receita, despesa e saldo) de UMA pessoa específica.
    public class TotalPorPessoaDto
    {
        public int PessoaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal TotalReceitas { get; set; }
        public decimal TotalDespesas { get; set; }
        public decimal Saldo { get; set; }
    }

    // Relatório completo: a lista de totais por pessoa + o total geral somando todo mundo.
    public class RelatorioTotaisDto
    {
        public List<TotalPorPessoaDto> Pessoas { get; set; } = new();
        public decimal TotalGeralReceitas { get; set; }
        public decimal TotalGeralDespesas { get; set; }
        public decimal SaldoGeral { get; set; }
    }
}
