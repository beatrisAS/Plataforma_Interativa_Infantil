using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using backend.ViewModels; // 1. IMPORTAR OS VIEWMODELS
using System.Security.Claims; // 2. IMPORTAR CLAIMS (para o bug fix)
using Microsoft.EntityFrameworkCore; // 3. IMPORTAR EF (para o bug fix)
using Microsoft.AspNetCore.Authorization; // 4. IMPORTAR AUTORIZAÇÃO

namespace backend.Controllers
{
    [Authorize(Roles = "crianca")] // 5. Proteger o controller
    public class AtividadeMvcController : Controller
    {
        private readonly AppDbContext _db;

        public AtividadeMvcController(AppDbContext db)
        {
            _db = db;
        }

        // --- CORREÇÃO 1: MÉTODO 'DETALHE' ---
        // Este método agora mapeia o Model para o ViewModel
        public IActionResult Detalhe(int id)
        {
            var activitiesJson = HttpContext.Session.GetString("UserActivities");
            if (string.IsNullOrEmpty(activitiesJson))
            {
                // Redireciona para o dashboard da criança, não para a "Home"
                return RedirectToAction("Index", "Crianca");
            }

            var activities = JsonSerializer.Deserialize<List<Atividade>>(activitiesJson);
            // Assumindo que 'atividades' da sessão contém as Questões e Alternativas
            var atividadeModel = activities?.FirstOrDefault(a => a.Id == id);

            if (atividadeModel == null)
            {
                return NotFound("A atividade não foi encontrada na sua sessão.");
            }

            // --- INÍCIO DA CORREÇÃO ---
            // Mapear o Atividade (Model) para o ResponderAtividadeViewModel (ViewModel)
            var viewModel = new ResponderAtividadeViewModel
            {
                Id = atividadeModel.Id,
                Titulo = atividadeModel.Titulo,
                Questoes = atividadeModel.Questoes.Select(q => new QuestaoViewModel
                {
                    Pergunta = q.Pergunta,
                    Explicacao = q.Explicacao,
                    Alternativas = q.Alternativas.Select(alt => new AlternativaViewModel
                    {
                        Texto = alt.Texto,
                        Correta = alt.Correta
                    }).ToList()
                }).ToList()
            };
            // --- FIM DA CORREÇÃO ---

            // Envia o 'viewModel' para a view, em vez do 'atividadeModel'
            return View("Atividade", viewModel);
        }

        // --- CORREÇÃO 2: MÉTODO 'SALVARRESULTADO' ---
        // Este método agora pega o ID do usuário logado
        [HttpPost]
        [ValidateAntiForgeryToken] // 6. Adicionar proteção AntiForgery
        [Route("/api/atividades/salvarresultado")]
        public async Task<IActionResult> SalvarResultado([FromBody] ResultadoPostModel model)
        {
            // --- INÍCIO DA CORREÇÃO DO BUG ---
            // Pega o ID do *usuário* logado (que é o ClaimTypes.NameIdentifier)
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out int usuarioId))
            {
                return Unauthorized("Usuário não autenticado.");
            }

            // Busca o perfil da criança usando o ID_USUARIO (Foreign Key)
            var crianca = await _db.Criancas.FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
            if (crianca == null)
            {
                return NotFound("Perfil da criança não encontrado.");
            }
            // --- FIM DA CORREÇÃO DO BUG ---

            var desempenho = (int)Math.Round((double)model.Acertos / model.TotalQuestoes * 100);
            var estrelasGanha = model.Acertos;

            crianca.Estrelas += estrelasGanha;
            
            var novaResposta = new RespostaAtividade
            {
                CriancaId = crianca.Id, // Usa o ID da criança encontrada
                AtividadeId = model.AtividadeId,
                Desempenho = desempenho,
                DataRealizacao = System.DateTime.UtcNow
            };

            _db.RespostasAtividades.Add(novaResposta);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Resultado salvo com sucesso!", estrelas = crianca.Estrelas });
        }
    }


    public class ResultadoPostModel
    {
        public int AtividadeId { get; set; }
        public int Acertos { get; set; }
        public int TotalQuestoes { get; set; }
    }
}