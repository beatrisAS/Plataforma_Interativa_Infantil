using Microsoft.AspNetCore.Mvc;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using backend.ViewModels;
using System.Collections.Generic;

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
            // --- CORREÇÃO DE NULIDADE ---
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdValue) || !int.TryParse(userIdValue, out var paiId))
            {
                // Se não encontrar o ID do usuário, redireciona para o logout
                return RedirectToAction("Logout", "Account");
            }
            // --- FIM DA CORREÇÃO ---

            var criancas = await _context.Criancas
                .Where(c => c.IdResponsavel == paiId)
                .Select(c => new CriancaProgressoViewModel
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Estrelas = c.Estrelas,
                    Respostas = _context.RespostasAtividades
                        .Where(r => r.CriancaId == c.Id)
                        .Include(r => r.Atividade)
                        .OrderByDescending(r => r.DataRealizacao)
                        .ToList()
                })
                .ToListAsync();
            
            return View(criancas);
        }
    }
}

