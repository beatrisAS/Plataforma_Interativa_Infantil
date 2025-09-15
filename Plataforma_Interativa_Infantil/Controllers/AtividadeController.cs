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

    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "professor")]
    [HttpPost]
    public async Task<IActionResult> Create(Atividade a)
    {
        _db.Atividades.Add(a);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = a.Id }, a);
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
        .Where(a => a.Ordem > atividadeAtual.Ordem)
        .OrderBy(a => a.Ordem)
        .FirstOrDefaultAsync();

    if (proxima == null)
        return Ok(new { mensagem = "Você concluiu todas as atividades! 🎉" });

    return Ok(proxima);
}

}
