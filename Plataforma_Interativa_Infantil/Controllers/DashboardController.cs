using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;
using System.Text.Json;
using System.Linq;
using System.Collections.Generic;
using backend.ViewModels; // Importante
using System;
using System.Threading.Tasks;

namespace backend.Controllers
{
    // Corrigido para usar o 'ActivityService'
    public class DashboardController : Controller
    {
        private readonly ActivityService _activityService;

        // Construtor padrão para injetar o serviço
        public DashboardController(ActivityService activityService)
        {
            _activityService = activityService;
        }

        public IActionResult Index()
        {
            // 1. Busca as atividades (Modelo)
            var atividadesModelo = GetActivitiesFromSession();

            // 2. Cria o MOCK da Criança
            var crianca = new Crianca
            {
                Id = 1, // Mock
                UsuarioId = 1, // Mock
                Nome = "Ana Silva (Teste)",
                DataNascimento = new DateTime(2015, 3, 10),
                Estrelas = 5,
                CodigoDeVinculo = "ABC123" // Mock
            };

            // 3. CORREÇÃO CONQUISTAS:
            // Crie 'List<ConquistaViewModel>' em vez de 'List<string>'
            var conquistas = new List<ConquistaViewModel>
            {
                new ConquistaViewModel
                {
                    Nome = "Completou a 1ª atividade!",
                    Descricao = "Você fez sua primeira lição.",
                    Icone = "bi-check-lg",
                    Desbloqueada = true // Mock: Desbloqueada
                },
                new ConquistaViewModel
                {
                    Nome = "Mestre da Matemática",
                    Descricao = "Terminou 3 lições de matemática.",
                    Icone = "bi-calculator",
                    Desbloqueada = false // Mock: Bloqueada
                }
            };

            // 4. CORREÇÃO ATIVIDADES:
            // Converta 'List<Atividade>' para 'List<AtividadeComStatusViewModel>'
            var atividadesParaView = atividadesModelo.Select((at, index) => new AtividadeComStatusViewModel
            {
                Atividade = at,
                // Mock: Marca a primeira atividade como concluída e as outras não
                Concluida = (index == 0) 
            }).ToList();

            // 5. Pega as categorias únicas (o seu ViewModel espera isso)
            var categorias = atividadesModelo.Select(a => a.Categoria).Distinct().ToList();

            // 6. Cria o ViewModel com os dados CORRETOS
            var viewModel = new CriancaDashboardViewModel
            {
                Crianca = crianca,
                Atividades = atividadesParaView, // <- Corrigido
                Conquistas = conquistas,        // <- Corrigido
                CategoriasUnicas = categorias
            };

            // Você disse que sua View se chama "CriancaDashboard"
            return View("CriancaDashboard", viewModel);
        }

        private List<Atividade> GetActivitiesFromSession()
        {
            var activitiesJson = HttpContext.Session.GetString("UserActivities");

            if (string.IsNullOrEmpty(activitiesJson))
            {
                // Usa o serviço injetado
                var newActivities = _activityService.GetRandomActivities(7); 
                HttpContext.Session.SetString("UserActivities", JsonSerializer.Serialize(newActivities));
                return newActivities; // O cast (List<Atividade>) era desnecessário
            }

            return JsonSerializer.Deserialize<List<Atividade>>(activitiesJson) ?? new List<Atividade>();
        }

   
    }
}