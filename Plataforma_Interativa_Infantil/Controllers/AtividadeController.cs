using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AtividadeController : ControllerBase
{
    private readonly AppDbContext _db;
    public AtividadeController(AppDbContext db) { _db = db; }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Atividades.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var u = await _db.Atividades.FindAsync(id);
        if (u == null) return NotFound();
        return Ok(u);
    }

 

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Atividade a)
    {
        var exists = await _db.Atividades.FindAsync(id);
        if (exists == null) return NotFound();
        exists.Titulo = a.Titulo;
        exists.Descricao = a.Descricao;
        exists.FaixaEtaria = a.FaixaEtaria;
        exists.Categoria = a.Categoria;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ex = await _db.Atividades.FindAsync(id);
        if (ex == null) return NotFound();
        _db.Atividades.Remove(ex);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("proxima/{atividadeId}")]
    public async Task<IActionResult> GetProximaAtividade(int atividadeId)
    {
        var atividadeAtual = await _db.Atividades.FirstOrDefaultAsync(a => a.Id == atividadeId);
        if (atividadeAtual == null)
            return NotFound("Atividade não encontrada.");

        var proxima = await _db.Atividades
            .Where(a => a.Id > atividadeAtual.Id)
    .OrderBy(a => a.Id)

            .FirstOrDefaultAsync();

        if (proxima == null)
            return Ok(new { mensagem = "Você concluiu todas as atividades! 🎉" });

        return Ok(proxima);
    }

    [HttpGet("porCategoria")]
    public async Task<IActionResult> GetPorCategoria(string categoria, string faixa = null)
    {
        var query = _db.Atividades.AsQueryable();

        if (!string.IsNullOrEmpty(categoria))
            query = query.Where(a => a.Categoria == categoria);

        if (!string.IsNullOrEmpty(faixa))
            query = query.Where(a => a.FaixaEtaria == faixa);

        var atividades = await query
            .Include(a => a.Questoes)
            .ThenInclude(q => q.Alternativas)
            .ToListAsync();

        return Ok(atividades);
    }

    [HttpPost]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "professor")]
    public async Task<IActionResult> Create([FromBody] Atividade a)
    {
        if (string.IsNullOrWhiteSpace(a.Titulo) || string.IsNullOrWhiteSpace(a.FaixaEtaria))
            return BadRequest("Título e Faixa Etária são obrigatórios.");

        _db.Atividades.Add(a);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = a.Id }, a);
    }

    [HttpGet("filtrar")]
    public async Task<IActionResult> Filtrar([FromQuery] string faixa, [FromQuery] string categoria)
    {
        var query = _db.Atividades.AsQueryable();
        if (!string.IsNullOrEmpty(faixa))
            query = query.Where(a => a.FaixaEtaria == faixa);
        if (!string.IsNullOrEmpty(categoria))
            query = query.Where(a => a.Categoria == categoria);

        var atividades = await query
            .OrderBy(a => a.Id)
            .Select(a => new
            {
                a.Id,
                a.Titulo,
                a.Descricao,
                a.FaixaEtaria,
                a.Categoria
            })
            .ToListAsync();

        return Ok(atividades);
    }

[HttpGet("dashboard")]
public async Task<IActionResult> DashboardAtividades([FromQuery] string faixa = null)
{
    var atividades = _db.Atividades.AsQueryable();
    if (!string.IsNullOrEmpty(faixa))
        atividades = atividades.Where(a => a.FaixaEtaria == faixa);

    var resultado = await atividades
        .OrderBy(a => a.Id)
        .Select(a => new {
            a.Id,
            a.Titulo,
            a.Categoria,
            a.FaixaEtaria
        })
        .ToListAsync();

    return Ok(resultado);
}



}
