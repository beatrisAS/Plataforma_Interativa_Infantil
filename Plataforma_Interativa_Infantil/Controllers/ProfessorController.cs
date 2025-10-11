using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using backend.ViewModels;
using backend.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace backend.Controllers
{
    [Authorize(Roles = "professor")]
    public class ProfessorController : Controller
    {
        private readonly AppDbContext _context;

        public ProfessorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // ✅ CORRECTION: Get the ID as a string first
            var professorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(professorIdString, out int professorId))
            {
                return Unauthorized("ID do professor inválido.");
            }

            // Now use the integer ID for filtering
            var todosAlunos = await _context.Criancas.ToListAsync();
            var todasAtividades = await _context.Atividades
                .Where(a => a.ProfessorId == professorId) // Use the int ID
                .OrderBy(a => a.Titulo)
                .ToListAsync();
            // ... (rest of the method is fine)

            var idsAtividadesDoProfessor = todasAtividades.Select(a => a.Id).ToList();

            var todasRespostas = await _context.RespostasAtividades
                .Where(r => idsAtividadesDoProfessor.Contains(r.AtividadeId))
                .Include(r => r.Atividade)
                .ToListAsync();
            
            var progressoDosAlunos = todosAlunos.Select(aluno =>
            {
                var respostasDoAluno = todasRespostas.Where(r => r.CriancaId == aluno.Id).ToList();
                var atividadesUnicasDoAluno = respostasDoAluno
                    .Select(r => r.Atividade)
                    .GroupBy(a => a.Id)
                    .Select(g => g.First())
                    .ToList();

                return new CriancaProgressoViewModel
                {
                    Id = aluno.Id,
                    Nome = aluno.Nome,
                    Estrelas = aluno.Estrelas,
                    DataNascimento = aluno.DataNascimento,
                    Respostas = respostasDoAluno,
                    AtividadesUnicas = atividadesUnicasDoAluno
                };
            }).ToList();
            
            var totalRespostas = todasRespostas.Count;
            double mediaGeral = totalRespostas > 0 ? todasRespostas.Average(r => r.Desempenho) : 0;

            var dashboardViewModel = new ProfessorDashboardViewModel
            {
                Alunos = progressoDosAlunos,
                AtividadesPublicadas = todasAtividades,
                TotalAlunos = todosAlunos.Count,
                TotalAtividades = todasAtividades.Count,
                RespostasRecebidas = totalRespostas,
                MediaGeral = (int)mediaGeral
            };

            return View(dashboardViewModel);
        }

        [HttpGet]
        public IActionResult CriarAtividade()
        {
            var model = new CriarAtividadeViewModel
            {
                Questoes = new List<QuestaoViewModel> { new QuestaoViewModel() }
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarAtividade(CriarAtividadeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

          
            var professorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(professorIdString, out int professorId))
            {
                ModelState.AddModelError("", "Não foi possível identificar o professor. Por favor, faça login novamente.");
                return View(model);
            }

            var novaAtividade = new Atividade
            {
                Titulo = model.Titulo,
                Descricao = model.Descricao,
                Categoria = model.Categoria,
                FaixaEtaria = model.FaixaEtaria,
                ProfessorId = professorId,
                Questoes = model.Questoes.Select(q => new Questao
                {
                    Pergunta = q.Pergunta,
                    Alternativas = q.Alternativas.Select(a => new Alternativa
                    {
                        Texto = a.Texto,
                        Correta = a.Correta
                    }).ToList()
                }).ToList()
            };

            _context.Atividades.Add(novaAtividade);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Atividade criada com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetDesempenhoPorMateriaData()
        {
         
            var professorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(professorIdString, out int professorId))
            {
                return Unauthorized("ID do professor inválido.");
            }

            var desempenho = await _context.RespostasAtividades
                .Include(r => r.Atividade)
                .Where(r => r.Atividade.ProfessorId == professorId) 
                .GroupBy(r => r.Atividade.Categoria)
                .Select(g => new
                {
                    Materia = g.Key,
                    DesempenhoMedio = g.Average(r => r.Desempenho)
                })
                .ToListAsync();

            return Json(desempenho);
        }
    }
}