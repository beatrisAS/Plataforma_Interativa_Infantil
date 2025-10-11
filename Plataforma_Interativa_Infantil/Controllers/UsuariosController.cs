using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController(AppDbContext db) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await db.Usuarios.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id) {
        var u = await db.Usuarios.FindAsync(id);
        if (u == null) return NotFound();
        return Ok(u);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Usuario u) {
        
        db.Usuarios.Add(u);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = u.Id }, u);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Usuario u) {
        var exists = await db.Usuarios.FindAsync(id);
        if (exists == null) return NotFound();

        exists.Nome = u.Nome;
        exists.Email = u.Email;
        exists.Perfil = u.Perfil;
        exists.Senha = u.Senha; 

        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) {
        var ex = await db.Usuarios.FindAsync(id);
        if (ex == null) return NotFound();

        db.Usuarios.Remove(ex);
        await db.SaveChangesAsync();
        return NoContent();
    }
}
