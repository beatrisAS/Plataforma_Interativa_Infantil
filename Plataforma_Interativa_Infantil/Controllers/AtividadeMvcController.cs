using Microsoft.AspNetCore.Mvc;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace backend.Controllers
{
    public class AtividadeMvcController : Controller
    {
        private readonly AppDbContext _db;
        public AtividadeMvcController(AppDbContext db) { _db = db; }


        public async Task<IActionResult> Detalhe(int id)
        {
            var atividade = await _db.Atividades
                .Include(a => a.Questoes)
                .ThenInclude(q => q.Alternativas)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (atividade == null) return NotFound();

            return View("Atividade", atividade);
        }
    
    
    public async Task<IActionResult> Proxima(int id)
        {
            var atividadeAtual = await _db.Atividades.FirstOrDefaultAsync(a => a.Id == id);
            if (atividadeAtual == null) return NotFound();

            var proxima = await _db.Atividades
                .Where(a => a.Ordem > atividadeAtual.Ordem)
                .OrderBy(a => a.Ordem)
                .FirstOrDefaultAsync();

            if (proxima == null)
                return View("Fim"); // 👈 você cria uma view de conclusão

            return RedirectToAction("Detalhe", new { id = proxima.Id });
        }

    }
}
