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
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)] // Adicionado para evitar cache
    public class ProfessorController : Controller
    {
        private readonly AppDbContext _context;

        public ProfessorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var professorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(professorIdString, out int professorId))
            {
                return Unauthorized("ID do professor inválido.");
            }

            // --- INÍCIO DA CORREÇÃO ---

            // 1. Busca os IDs dos alunos vinculados (como antes)
            var idsAlunosVinculados = await _context.ProfessorAlunos
                .Where(pa => pa.ProfessorId == professorId)
                .Select(pa => pa.CriancaId)
                .ToListAsync();

            // 2. Busca os perfis dos alunos (como antes)
            var alunosDoProfessor = await _context.Criancas
                .Where(c => idsAlunosVinculados.Contains(c.Id))
                .ToListAsync();

            // 3. Busca as atividades CRIADAS por este professor (para as estatísticas e lista "Atividades Publicadas")
            var atividadesDoProfessor = await _context.Atividades
                .Where(a => a.ProfessorId == professorId)
                .OrderBy(a => a.Titulo)
                .ToListAsync();
            var idsAtividadesDoProfessor = atividadesDoProfessor.Select(a => a.Id).ToList();

            // 4. Busca as respostas APENAS das atividades do professor (PARA AS ESTATÍSTICAS)
            var respostasDasAtividadesDoProfessor = await _context.RespostasAtividades
                .Where(r => idsAtividadesDoProfessor.Contains(r.AtividadeId))
                .Include(r => r.Atividade) 
                .ToListAsync();
            
            // 5. Busca TODAS as respostas dos alunos vinculados (PARA O PROGRESSO INDIVIDUAL)
            var respostasDeTodosOsAlunos = await _context.RespostasAtividades
                .Where(r => idsAlunosVinculados.Contains(r.CriancaId)) // <-- A MUDANÇA ESTÁ AQUI
                .Include(r => r.Atividade)
                .ToListAsync();

            // 6. Monta o progresso dos alunos usando a lista de TODAS as respostas deles
            var progressoDosAlunos = alunosDoProfessor.Select(aluno =>
            {
                // Usa a lista 'respostasDeTodosOsAlunos' para encontrar o progresso
                var respostasDoAluno = respostasDeTodosOsAlunos.Where(r => r.CriancaId == aluno.Id).ToList();
                
                var atividadesUnicasDoAluno = respostasDoAluno
                    .Select(r => r.Atividade) 
                    .Where(a => a != null)     
                    .GroupBy(a => a.Id)
                    .Select(g => g.First())
                    .ToList();

                return new CriancaProgressoViewModel
                {
                    Id = aluno.Id,
                    Nome = aluno.Nome,
                    Estrelas = aluno.Estrelas,
                    DataNascimento = aluno.DataNascimento,
                    Respostas = respostasDoAluno, // Passa as respostas corretas
                    AtividadesUnicas = atividadesUnicasDoAluno
                };
            }).ToList();
            
            // 7. Calcula as estatísticas do professor usando APENAS as respostas das suas próprias atividades
            var totalRespostas = respostasDasAtividadesDoProfessor.Count;
            double mediaGeral = totalRespostas > 0 ? respostasDasAtividadesDoProfessor.Average(r => r.Desempenho) : 0;

            var dashboardViewModel = new ProfessorDashboardViewModel
            {
                Alunos = progressoDosAlunos, // <-- Agora contém o progresso real
                AtividadesPublicadas = atividadesDoProfessor,
                TotalAlunos = alunosDoProfessor.Count,
                TotalAtividades = atividadesDoProfessor.Count,
                RespostasRecebidas = totalRespostas, // Estatística do professor
                MediaGeral = (int)mediaGeral // Estatística do professor
            };

            // --- FIM DA CORREÇÃO ---

            return View(dashboardViewModel);
        }
        
       
        [HttpGet]
        public IActionResult VincularAluno()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VincularAluno(string codigoDeVinculo)
        {
            if (string.IsNullOrEmpty(codigoDeVinculo))
            {
                ModelState.AddModelError("", "O código de vínculo é obrigatório.");
                return View();
            }

            var crianca = await _context.Criancas
                .FirstOrDefaultAsync(c => c.CodigoDeVinculo == codigoDeVinculo.Trim());

            if (crianca == null)
            {
                ModelState.AddModelError("", "Nenhum aluno encontrado com este código.");
                return View();
            }

            var professorIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(professorIdString, out int professorId))
            {
                return Unauthorized("ID do professor inválido.");
            }

            var vinculoExistente = await _context.ProfessorAlunos
                .AnyAsync(pa => pa.ProfessorId == professorId && pa.CriancaId == crianca.Id);

            if (vinculoExistente)
            {
                ModelState.AddModelError("", "Este aluno já está vinculado à sua conta.");
                return View();
            }

            var novoVinculo = new ProfessorAluno
            {
                ProfessorId = professorId,
                CriancaId = crianca.Id
            };

            _context.ProfessorAlunos.Add(novoVinculo);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Aluno '{crianca.Nome}' vinculado com sucesso!";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult CriarAtividade()
        {
            var model = new CriarAtividadeViewModel
            {
                Titulo = string.Empty,
                Descricao = string.Empty,
                Categoria = string.Empty,
                FaixaEtaria = string.Empty,
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

            // Esta consulta está correta, pois o gráfico deve refletir as atividades do professor
            var desempenho = await _context.RespostasAtividades
                .Include(r => r.Atividade)
                .Where(r => r.Atividade != null && r.Atividade.ProfessorId == professorId) 
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
