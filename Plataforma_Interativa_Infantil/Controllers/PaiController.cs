using Microsoft.AspNetCore.Mvc;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using backend.ViewModels;
using System.Collections.Generic;
using backend.Models; 

namespace backend.Controllers
{
    [Authorize(Roles = "pai")]
    public class PaiController : Controller
    {
        private readonly AppDbContext _context;

        public PaiController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdValue) || !int.TryParse(userIdValue, out var paiId))
            {
                return RedirectToAction("Logout", "Account");
            }

            var criancasDoPai = await _context.Criancas
                .Where(c => c.IdResponsavel == paiId)
                .ToListAsync();

            var progressoDasCriancas = new List<CriancaProgressoViewModel>();

            foreach (var crianca in criancasDoPai)
            {
                var respostas = await _context.RespostasAtividades
                    .Where(r => r.CriancaId == crianca.Id)
                    .Include(r => r.Atividade) 
                    .ToListAsync();
                
                var atividadesUnicas = respostas
                    .Select(r => r.Atividade)
                    .GroupBy(a => a.Id)
                    .Select(g => g.First())
                    .OrderBy(a => a.Categoria)
                    .ToList();
                
                progressoDasCriancas.Add(new CriancaProgressoViewModel
                {
                    Id = crianca.Id,
                    Nome = crianca.Nome,
                    Estrelas = crianca.Estrelas,
                    DataNascimento = crianca.DataNascimento, 
                    Respostas = respostas,
                    AtividadesUnicas = atividadesUnicas
                });
            }
            
            return View(progressoDasCriancas);
        }
    }
}