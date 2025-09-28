using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using backend.Services;
using backend.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Authorize(Roles = "crianca")] // Protege o dashboard para que apenas crianças logadas acessem
    public class CriancaController : Controller
    {
        private readonly AppDbContext _db;
        private readonly ActivityService _activityService;
        private readonly AchievementService _achievementService;

        public CriancaController(AppDbContext db, ActivityService activityService, AchievementService achievementService)
        {
            _db = db;
            _activityService = activityService;
            _achievementService = achievementService;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index()
        {
            // TODO: Substituir ID fixo pela lógica de usuário logado a partir do Claim
            var criancaId = 1; 
            var crianca = await _db.Criancas.FindAsync(criancaId);

            if (crianca == null)
            {
                crianca = new Crianca { Id = criancaId, Nome = "Visitante", Estrelas = 0, DataNascimento = System.DateTime.Now.AddYears(-8) };
            }

            var atividadesDinamicas = GetActivitiesFromSession();
            var atividadesFixas = await _db.Atividades.ToListAsync();
            
            var todasAtividades = new List<Atividade>();
            todasAtividades.AddRange(atividadesDinamicas);
            todasAtividades.AddRange(atividadesFixas);

            var respostasSalvas = await _db.RespostasAtividades.Where(r => r.CriancaId == crianca.Id).ToListAsync();
            var conquistas = _achievementService.CheckAchievements(crianca, respostasSalvas, atividadesDinamicas);

            var viewModel = new CriancaDashboardViewModel
            {
                Crianca = crianca,
                Atividades = todasAtividades.GroupBy(a => a.Categoria).Select(g => g.First()).ToList(),
                Conquistas = conquistas
            };

            // Por convenção, isso irá procurar a view em /Views/Crianca/Index.cshtml
            return View(viewModel);
        }
        
        private List<Atividade> GetActivitiesFromSession()
        {
            var activitiesJson = HttpContext.Session.GetString("UserActivities");
            if (string.IsNullOrEmpty(activitiesJson))
            {
                var newActivities = _activityService.GetRandomActivities(7);
                HttpContext.Session.SetString("UserActivities", JsonSerializer.Serialize(newActivities));
                return newActivities;
            }
            return JsonSerializer.Deserialize<List<Atividade>>(activitiesJson) ?? new List<Atividade>();
        }
    }
}