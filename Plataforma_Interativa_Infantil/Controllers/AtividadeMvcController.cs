using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace backend.Controllers
{
    public class AtividadeMvcController : Controller
    {
        private readonly AppDbContext _db;

        public AtividadeMvcController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Detalhe(int id)
        {
            var activitiesJson = HttpContext.Session.GetString("UserActivities");
            if (string.IsNullOrEmpty(activitiesJson))
            {
                return RedirectToAction("CriancaDashboard", "Home");
            }

            var activities = JsonSerializer.Deserialize<List<Atividade>>(activitiesJson);
            var atividade = activities?.FirstOrDefault(a => a.Id == id);

            if (atividade == null)
            {
                return NotFound("A atividade não foi encontrada na sua sessão.");
            }

            return View("Atividade", atividade);
        }

        [HttpPost]
        [Route("/api/atividades/salvarresultado")]
        public async Task<IActionResult> SalvarResultado([FromBody] ResultadoPostModel model)
        {
    
            var criancaId = 1;
            var crianca = await _db.Criancas.FindAsync(criancaId);
            if (crianca == null) return NotFound("Criança não encontrada.");

            var desempenho = (int)Math.Round((double)model.Acertos / model.TotalQuestoes * 100);
            var estrelasGanha = model.Acertos;

            crianca.Estrelas += estrelasGanha;
            
            var novaResposta = new RespostaAtividade
            {
                CriancaId = criancaId,
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
