using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

public class AtividadeMvcController : Controller
{
    private readonly AppDbContext _db;
    public AtividadeMvcController(AppDbContext db) { _db = db; }

    // GET: /AtividadeMvc/Detalhe/5
    public async Task<IActionResult> Detalhe(int id)
    {
        // Inclui as questões e alternativas (ajuste conforme seus relacionamentos)
        var atividade = await _db.Atividades
            .Include(a => a.Questoes)
            .ThenInclude(q => q.Alternativas)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (atividade == null) return NotFound();

        return View("Atividade", atividade);
    }
}
