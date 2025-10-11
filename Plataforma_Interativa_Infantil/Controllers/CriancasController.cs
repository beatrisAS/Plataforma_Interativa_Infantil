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
    [Authorize(Roles = "crianca")]
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
           
            var criancaId = 1; 
            var crianca = await _db.Criancas.FindAsync(criancaId);

            if (crianca == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var atividadesDinamicas = GetActivitiesFromSession();
            var atividadesFixas = await _db.Atividades.ToListAsync();
            
            var todasAtividades = new List<Atividade>();
            todasAtividades.AddRange(atividadesDinamicas);
            todasAtividades.AddRange(atividadesFixas);

     
            var atividadesUnicas = todasAtividades.GroupBy(a => a.Id).Select(g => g.First()).ToList();

           
            var atividadesFinais = atividadesUnicas.Take(7).ToList();

            
            var categoriasReais = atividadesFinais
                                      .Select(a => a.Categoria)
                                      .Distinct()
                                      .OrderBy(c => c)
                                      .ToList();

            var respostasSalvas = await _db.RespostasAtividades.Where(r => r.CriancaId == crianca.Id).ToListAsync();
            var conquistas = _achievementService.CheckAchievements(crianca, respostasSalvas, atividadesDinamicas);

            var viewModel = new CriancaDashboardViewModel
            {
                Crianca = crianca,
               
                Atividades = atividadesFinais,
                CategoriasUnicas = categoriasReais,
                Conquistas = conquistas
            };

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