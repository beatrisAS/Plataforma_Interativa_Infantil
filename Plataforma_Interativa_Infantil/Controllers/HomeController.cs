using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

   
        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index() => View();

        public IActionResult CriancaDashboard() => View();

        public IActionResult PaiDashboard() => View();

        public IActionResult ProfessorDashboard() => View();

        public async Task<IActionResult> Atividade(int id)
        {
            var atividade = await _db.Atividades.FirstOrDefaultAsync(a => a.Id == id);
            if (atividade == null)
                return NotFound();

            return View(atividade);
        }
    }
}
