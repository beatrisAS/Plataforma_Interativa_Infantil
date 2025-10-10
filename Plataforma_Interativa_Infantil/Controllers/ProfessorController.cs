using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using backend.ViewModels;
using backend.Models;
using System.Collections.Generic;
using System.Security.Claims; // Necessário para pegar o usuário logado

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
            // Busca todos os dados necessários de forma otimizada
            var todosAlunos = await _context.Criancas.ToListAsync();
            var todasRespostas = await _context.RespostasAtividades
                .Include(r => r.Atividade)
                .ToListAsync();
            var todasAtividades = await _context.Atividades
                .OrderBy(a => a.Titulo)
                .ToListAsync();

            // Monta o progresso de cada aluno em memória
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

            // Preenche o ViewModel principal com todos os dados para a página
            var dashboardViewModel = new ProfessorDashboardViewModel
            {
                Alunos = progressoDosAlunos,
                AtividadesPublicadas = todasAtividades,
                TotalAlunos = todosAlunos.Count,
                TotalAtividades = todasAtividades.Count
            };

            return View(dashboardViewModel);
        }

        // GET: Exibe o formulário para criar uma nova atividade
        [HttpGet]
        public IActionResult CriarAtividade()
        {
            var model = new CriarAtividadeViewModel
            {
                Questoes = new List<QuestaoViewModel> { new QuestaoViewModel() }
            };
            return View(model);
        }

        // POST: Salva a nova atividade criada
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarAtividade(CriarAtividadeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var novaAtividade = new Atividade
            {
                Titulo = model.Titulo,
                Descricao = model.Descricao,
                Categoria = model.Categoria,
                FaixaEtaria = model.FaixaEtaria,
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
    }
}