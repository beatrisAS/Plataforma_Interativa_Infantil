using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using backend.ViewModels;
using backend.Models;
using System.Collections.Generic;

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
            var todosAlunos = await _context.Criancas
                .Select(c => new CriancaProgressoViewModel
                {
                    Id = c.Id,
                    Nome = c.Nome,
                    Estrelas = c.Estrelas,
                    Respostas = _context.RespostasAtividades
                        .Where(r => r.CriancaId == c.Id)
                        .Include(r => r.Atividade)
                        .ToList()
                })
                .ToListAsync();

            return View(todosAlunos);
        }

        // GET: Exibe o formulário para criar uma nova atividade
        [HttpGet]
        public IActionResult CriarAtividade()
        {
            // Inicia o formulário com uma questão e quatro alternativas
            var model = new CriarAtividadeViewModel
            {
                Questoes = new List<QuestaoViewModel> { new QuestaoViewModel() }
            };
            return View(model);
        }

        // POST: Salva a nova atividade criada pelo professor no banco de dados
        [HttpPost]
        public async Task<IActionResult> CriarAtividade(CriarAtividadeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Se o modelo não for válido, retorna para o formulário com os erros
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

