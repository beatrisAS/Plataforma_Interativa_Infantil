using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComentariosController : ControllerBase {
    private readonly AppDbContext _db;
    public ComentariosController(AppDbContext db) { _db = db; }

    [HttpPost]
    [Authorize] // any authenticated user can submit, will be Pending
    public async Task<IActionResult> Post([FromBody] Comentario c) {
        c.Status = ComentarioStatus.Pending;
        c.CriadoEm = System.DateTime.UtcNow;
        _db.Comentarios.Add(c);
        await _db.SaveChangesAsync();
        return Ok(new { message = "Comentário enviado para moderação" });
    }

    [HttpGet("pending")]
    [Authorize(Roles = "professor")]
    public async Task<IActionResult> Pending() {
        var list = await _db.Comentarios.Where(x => x.Status == ComentarioStatus.Pending).ToListAsync();
        return Ok(list);
    }

    [HttpPost("moderate/{id}")]
    [Authorize(Roles = "professor")]
    public async Task<IActionResult> Moderate(int id, [FromQuery] string action) {
        var c = await _db.Comentarios.FindAsync(id);
        if (c == null) return NotFound();
        if (action == "approve") c.Status = ComentarioStatus.Approved;
        else if (action == "reject") c.Status = ComentarioStatus.Rejected;
        else return BadRequest();
        await _db.SaveChangesAsync();
        return Ok(c);
    }

    [HttpGet("foratividade/{atividadeId}")]
    public async Task<IActionResult> ForAtividade(int atividadeId) {
        var list = await _db.Comentarios.Where(x => x.AtividadeId==atividadeId && x.Status==ComentarioStatus.Approved).ToListAsync();
        return Ok(list);
    }
}
