namespace GastosResidenciais.Api.Models
{
    // Enum que representa o tipo de uma transação financeira.
    // Usar enum aqui garante que só existam esses dois valores possíveis,
    // evitando erros de digitação como "despesa", "Despesa", "DESPESA" etc.
    public enum TipoTransacao
    {
        Receita = 0,
        Despesa = 1
    }
}
