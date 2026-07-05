using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.Dtos;
using GastosResidenciais.Api.Models;

namespace GastosResidenciais.Api.Controllers
{
    // Controller responsável pelo cadastro de pessoas.
    // Endpoints disponíveis: GET, POST e DELETE em /api/pessoas
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : ControllerBase
    {
        private readonly AppDbContext _context;

        // O DbContext é "injetado" automaticamente pelo ASP.NET Core (Dependency Injection).
        public PessoasController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/pessoas
        // Lista todas as pessoas cadastradas, ordenadas por nome.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaDto>>> Listar()
        {
            var pessoas = await _context.Pessoas
                .OrderBy(p => p.Nome)
                .Select(p => new PessoaDto
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Idade = p.Idade,
                    EhMenorDeIdade = p.Idade < 18
                })
                .ToListAsync();

            return Ok(pessoas);
        }

        // POST api/pessoas
        // Cria uma nova pessoa. O Id é gerado automaticamente pelo banco (autoincrement),
        // então nunca recebemos o Id do front-end aqui.
        [HttpPost]
        public async Task<ActionResult<PessoaDto>> Criar(CriarPessoaDto dto)
        {
            // Validações básicas de negócio (além das anotações de [Required] no modelo).
            if (string.IsNullOrWhiteSpace(dto.Nome))
                return BadRequest("O nome é obrigatório.");

            if (dto.Idade < 0)
                return BadRequest("A idade não pode ser negativa.");

            var pessoa = new Pessoa
            {
                Nome = dto.Nome.Trim(),
                Idade = dto.Idade
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync(); // é aqui que o Id é gerado pelo banco

            var resultado = new PessoaDto
            {
                Id = pessoa.Id,
                Nome = pessoa.Nome,
                Idade = pessoa.Idade,
                EhMenorDeIdade = pessoa.EhMenorDeIdade
            };

            // 201 Created, com o link para o próprio recurso criado.
            return CreatedAtAction(nameof(Listar), new { id = pessoa.Id }, resultado);
        }

        // DELETE api/pessoas/5
        // Remove uma pessoa do cadastro.
        //
        // REGRA DE NEGÓCIO: ao deletar a pessoa, todas as transações dela também
        // são deletadas. Isso NÃO é feito manualmente aqui: configuramos no
        // AppDbContext (OnDelete Cascade) para o próprio banco cuidar disso,
        // de forma automática e segura (evita esquecer de apagar em algum lugar).
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);

            if (pessoa == null)
                return NotFound($"Pessoa com id {id} não encontrada.");

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent(); // 204: deu certo, mas não tem corpo pra devolver
        }
    }
}
