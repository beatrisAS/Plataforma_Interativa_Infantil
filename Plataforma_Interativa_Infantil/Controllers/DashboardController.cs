using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using backend.ViewModels;
using System;
using System.Threading.Tasks;

namespace backend.Controllers
{
    public class DashboardController(ActivityService activityService) : Controller
    {
        public IActionResult Index()
        {
            var activities = GetActivitiesFromSession();

           
            var crianca = new Crianca
            {
                Nome = "Ana Silva",
                DataNascimento = new DateTime(2015, 3, 10),
                Estrelas = 5
            };
            var conquistas = new List<string> { "Completou a 1ª atividade!", "Mestre da Matemática" };

           
            var viewModel = new CriancaDashboardViewModel
            {
                Crianca = crianca,
                Atividades = activities,
                Conquistas = conquistas
            };

            return View("CriancaDashboard", viewModel);
        }


        private List<Atividade> GetActivitiesFromSession()
        {
        
            var activitiesJson = HttpContext.Session.GetString("UserActivities");

          
            if (string.IsNullOrEmpty(activitiesJson))
            {
              
                var newActivities = activityService.GetRandomActivities(7);

          
                HttpContext.Session.SetString("UserActivities", JsonSerializer.Serialize(newActivities));
                return (List<Atividade>)newActivities;
            }

            return JsonSerializer.Deserialize<List<Atividade>>(activitiesJson) ?? new List<Atividade>();
        }
    }

}