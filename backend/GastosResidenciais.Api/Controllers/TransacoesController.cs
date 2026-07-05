using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.Dtos;
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Controllers
{
    // Controller responsável pelo cadastro de transações.
    // Só precisa de criação e listagem (o enunciado não pede editar/deletar).
    [ApiController]
    [Route("api/[controller]")]
    public class TransacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransacoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/transacoes
        // Lista todas as transações, já trazendo o nome da pessoa junto
        // (usamos Include para carregar os dados da Pessoa relacionada).
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransacaoDto>>> Listar()
        {
            var transacoes = await _context.Transacoes
                .Include(t => t.Pessoa)
                .OrderByDescending(t => t.Id) // mais recentes primeiro
                .Select(t => new TransacaoDto
                {
                    Id = t.Id,
                    Descricao = t.Descricao,
                    Valor = t.Valor,
                    Tipo = t.Tipo,
                    PessoaId = t.PessoaId,
                    PessoaNome = t.Pessoa != null ? t.Pessoa.Nome : ""
                })
                .ToListAsync();

            return Ok(transacoes);
        }

        // POST api/transacoes
        // Cria uma nova transação, aplicando as regras de negócio do enunciado.
        [HttpPost]
        public async Task<ActionResult<TransacaoDto>> Criar(CriarTransacaoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Descricao))
                return BadRequest("A descrição é obrigatória.");

            if (dto.Valor <= 0)
                return BadRequest("O valor da transação deve ser maior que zero.");

            // REGRA: "Esse valor [PessoaId] precisa existir no cadastro de pessoa."
            // Aqui é onde garantimos isso: se não achar a pessoa, nem deixa criar a transação.
            var pessoa = await _context.Pessoas.FindAsync(dto.PessoaId);
            if (pessoa == null)
                return BadRequest($"Não existe pessoa cadastrada com o id {dto.PessoaId}.");

            // REGRA: "Caso a pessoa informada seja menor de idade (menor de 18 anos),
            // apenas despesas poderão ser cadastradas."
            // Ou seja: menor de idade + tipo Receita = não pode.
            if (pessoa.EhMenorDeIdade && dto.Tipo == TipoTransacao.Receita)
            {
                return BadRequest(
                    "Pessoas menores de idade só podem ter despesas cadastradas, não receitas.");
            }

            var transacao = new Transacao
            {
                Descricao = dto.Descricao.Trim(),
                Valor = dto.Valor,
                Tipo = dto.Tipo,
                PessoaId = dto.PessoaId
            };

            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();

            var resultado = new TransacaoDto
            {
                Id = transacao.Id,
                Descricao = transacao.Descricao,
                Valor = transacao.Valor,
                Tipo = transacao.Tipo,
                PessoaId = transacao.PessoaId,
                PessoaNome = pessoa.Nome
            };

            return CreatedAtAction(nameof(Listar), new { id = transacao.Id }, resultado);
        }
    }
}
