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
            // --- 1. Geração Dinâmica de Atividades ---
            // Esta função usa o ActivityService para criar 7 atividades aleatórias
            // e as guarda na sessão para a criança não perdê-las se recarregar a página.
            var activities = GetActivitiesFromSession();

            // --- 2. Dados da Criança e Conquistas (Exemplo) ---
            // Como não estamos usando o banco aqui, usamos dados de exemplo.
            // A lógica real de login e conquistas pode ser adicionada depois.
            var crianca = new Crianca
            {
                Nome = "Ana Silva",
                DataNascimento = new DateTime(2015, 3, 10),
                Estrelas = 5
            };
            var conquistas = new List<string> { "Completou a 1ª atividade!", "Mestre da Matemática" };

            // --- 3. Montagem do ViewModel para a tela ---
            var viewModel = new CriancaDashboardViewModel
            {
                Crianca = crianca,
                Atividades = activities,
                Conquistas = conquistas
            };

            return View("CriancaDashboard", viewModel);
        }

        // Esta é a função principal que garante que as atividades sejam geradas dinamicamente
        private List<Atividade> GetActivitiesFromSession()
        {
            // Tenta buscar atividades da "memória de curto prazo" (Sessão)
            var activitiesJson = HttpContext.Session.GetString("UserActivities");

            // Se não encontrar nada, significa que é a primeira visita
            if (string.IsNullOrEmpty(activitiesJson))
            {
                // **AQUI A MÁGICA ACONTECE**
                // O sistema pede ao ActivityService para CRIAR 7 atividades novas
                var newActivities = activityService.GetRandomActivities(7);

                // Guarda as atividades recém-criadas na sessão
                HttpContext.Session.SetString("UserActivities", JsonSerializer.Serialize(newActivities));
                return (List<Atividade>)newActivities;
            }

            // Se já existiam atividades na sessão, apenas as retorna
            return JsonSerializer.Deserialize<List<Atividade>>(activitiesJson) ?? new List<Atividade>();
        }
    }

}