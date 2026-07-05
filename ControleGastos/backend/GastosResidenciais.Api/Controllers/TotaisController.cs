using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.Dtos;
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Controllers
{
    // Controller responsável pela consulta de totais.
    // Só tem um endpoint: GET api/totais.
    [ApiController]
    [Route("api/[controller]")]
    public class TotaisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TotaisController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/totais
        //
        // Monta o relatório pedido no enunciado:
        // - Para cada pessoa: total de receitas, total de despesas e saldo (receita - despesa).
        // - No final: o total geral somando todas as pessoas.
        [HttpGet]
        public async Task<ActionResult<RelatorioTotaisDto>> Consultar()
        {
            // Carrega todas as pessoas já junto com as transações de cada uma
            // (Include evita que eu precise fazer uma query por pessoa dentro do loop).
            var pessoas = await _context.Pessoas
                .Include(p => p.Transacoes)
                .OrderBy(p => p.Nome)
                .ToListAsync();

            var relatorio = new RelatorioTotaisDto();

            foreach (var pessoa in pessoas)
            {
                var totalReceitas = pessoa.Transacoes
                    .Where(t => t.Tipo == TipoTransacao.Receita)
                    .Sum(t => t.Valor);

                var totalDespesas = pessoa.Transacoes
                    .Where(t => t.Tipo == TipoTransacao.Despesa)
                    .Sum(t => t.Valor);

                relatorio.Pessoas.Add(new TotalPorPessoaDto
                {
                    PessoaId = pessoa.Id,
                    Nome = pessoa.Nome,
                    TotalReceitas = totalReceitas,
                    TotalDespesas = totalDespesas,
                    Saldo = totalReceitas - totalDespesas
                });

                // Vou acumulando os totais gerais enquanto percorro cada pessoa,
                // assim não preciso rodar outro loop depois só para somar tudo de novo.
                relatorio.TotalGeralReceitas += totalReceitas;
                relatorio.TotalGeralDespesas += totalDespesas;
            }

            relatorio.SaldoGeral = relatorio.TotalGeralReceitas - relatorio.TotalGeralDespesas;

            return Ok(relatorio);
        }
    }
}
