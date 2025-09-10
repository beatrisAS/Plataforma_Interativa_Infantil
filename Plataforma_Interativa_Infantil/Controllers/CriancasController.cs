using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CriancasController : ControllerBase {
    private readonly AppDbContext _db;
    public CriancasController(AppDbContext db) { _db = db; }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _db.Criancas.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) {
        var u = await _db.Criancas.FindAsync(id);
        if (u == null) return NotFound();
        return Ok(u);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Crianca c) {
        _db.Criancas.Add(c);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = c.Id }, c);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Crianca c) {
        var exists = await _db.Criancas.FindAsync(id);
        if (exists == null) return NotFound();
        exists.Nome = c.Nome;
        exists.DataNascimento = c.DataNascimento;
        exists.Genero = c.Genero;
        exists.IdResponsavel = c.IdResponsavel;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        var ex = await _db.Criancas.FindAsync(id);
        if (ex == null) return NotFound();
        _db.Criancas.Remove(ex);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
