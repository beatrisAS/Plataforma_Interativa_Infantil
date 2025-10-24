using Microsoft.AspNetCore.Mvc;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using backend.ViewModels; // <-- Importa os novos ViewModels
using System.Collections.Generic;
using backend.Models;
using System;

namespace backend.Controllers
{
    [Authorize(Roles = "pai")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)] // Evita cache
    public class PaiController : Controller
    {
        private readonly AppDbContext _context;

        public PaiController(AppDbContext context)
        {
            _context = context;
        }

        // --- MÉTODO INDEX ATUALIZADO ---
        public async Task<IActionResult> Index()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var paiId))
            {
                return RedirectToAction("Logout", "Account");
            }

            // Busca a PRIMEIRA criança vinculada a este pai
            var crianca = await _context.Criancas
                .Where(c => c.IdResponsavel == paiId)
                .FirstOrDefaultAsync();

            var viewModel = new PaiDashboardViewModel();

            if (crianca != null)
            {
                // Busca TODAS as atividades da plataforma (ou as relevantes)
                var todasAtividades = await _context.Atividades.ToListAsync();

                // Busca TODAS as respostas desta criança
                var respostasCrianca = await _context.RespostasAtividades
                    .Where(r => r.CriancaId == crianca.Id)
                    .Include(r => r.Atividade) // Inclui dados da atividade
                    .OrderByDescending(r => r.DataRealizacao) // Mais recentes primeiro
                    .ToListAsync();

                var atividadesRealizadas = new List<AtividadeRealizadaViewModel>();
                var atividadesPendentes = new List<AtividadePendenteViewModel>();

                // Processa cada atividade da plataforma
                foreach (var atividade in todasAtividades)
                {
                    // Encontra as respostas da criança para ESTA atividade
                    var respostasParaEstaAtividade = respostasCrianca
                        .Where(r => r.AtividadeId == atividade.Id)
                        .ToList();

                    if (respostasParaEstaAtividade.Any())
                    {
                        // Se há respostas, a atividade foi realizada
                        var respostaMaisRecente = respostasParaEstaAtividade.First(); // Já ordenamos por data
                        
                        // Assumindo 5 questões por atividade para calcular acertos
                        int totalQuestoes = 5; 
                        int acertos = (int)Math.Round((double)respostaMaisRecente.Desempenho / 100 * totalQuestoes);

                        atividadesRealizadas.Add(new AtividadeRealizadaViewModel
                        {
                            AtividadeId = atividade.Id,
                            Titulo = atividade.Titulo,
                            Categoria = atividade.Categoria,
                            Nota = respostaMaisRecente.Desempenho,
                            Acertos = acertos,
                            TotalQuestoes = totalQuestoes,
                            Tentativas = respostasParaEstaAtividade.Count
                        });
                    }
                    else
                    {
                        // Se não há respostas, a atividade está pendente
                        atividadesPendentes.Add(new AtividadePendenteViewModel
                        {
                            AtividadeId = atividade.Id,
                            Titulo = atividade.Titulo,
                            Categoria = atividade.Categoria
                        });
                    }
                }

                // Calcula a média por matéria para o gráfico
                var mediaPorMateria = atividadesRealizadas
                    .GroupBy(a => a.Categoria)
                    .Select(g => new ChartDataViewModel
                    {
                        Label = g.Key,
                        Value = (int)g.Average(item => item.Nota)
                    })
                    .OrderBy(c => c.Label)
                    .ToList();
                
                // Calcula a idade da criança
                var hoje = DateTime.Today;
                var idade = hoje.Year - crianca.DataNascimento.Year;
                if (crianca.DataNascimento.Date > hoje.AddYears(-idade)) idade--;

                // Preenche os detalhes da criança no ViewModel principal
                viewModel.CriancaDetalhe = new CriancaDetalheViewModel
                {
                    Id = crianca.Id,
                    Nome = crianca.Nome,
                    Idade = idade,
                    AtividadesRealizadas = atividadesRealizadas.OrderBy(a => a.Categoria).ToList(),
                    AtividadesPendentes = atividadesPendentes.OrderBy(a => a.Categoria).ToList(),
                    MediaPorMateria = mediaPorMateria
                };
            }
            // Se crianca == null, o viewModel.TemCriancasVinculadas será false

            return View(viewModel);
        }

        // --- AÇÕES DE VINCULAR CRIANÇA (sem alteração) ---
        [HttpGet]
        public IActionResult VincularCrianca()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VincularCrianca(string codigoDeVinculo)
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
                ModelState.AddModelError("", "Nenhuma criança encontrada com este código.");
                return View();
            }
            
            if (crianca.IdResponsavel != null)
            {
                // Verifica se já não está vinculada a ESTE pai para evitar erro
                var userIdValueCheck = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (int.TryParse(userIdValueCheck, out var paiIdCheck) && crianca.IdResponsavel == paiIdCheck) {
                     TempData["SuccessMessage"] = $"'{crianca.Nome}' já está vinculado à sua conta.";
                     return RedirectToAction("Index");
                } else {
                     ModelState.AddModelError("", "Esta criança já está vinculada a outro responsável.");
                     return View();
                }
            }

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var paiId))
            {
                return RedirectToAction("Logout", "Account");
            }

            crianca.IdResponsavel = paiId;

            try
            {
                _context.Criancas.Update(crianca);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"'{crianca.Nome}' foi vinculado à sua conta com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao vincular: " + ex.Message);
                return View();
            }
        }
    }
}
