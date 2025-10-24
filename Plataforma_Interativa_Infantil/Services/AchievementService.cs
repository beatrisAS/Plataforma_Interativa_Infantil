using backend.Models;
using backend.ViewModels; 
using System.Collections.Generic;
using System.Linq;

namespace backend.Services
{
    public class AchievementService
    {
        
        private List<ConquistaViewModel> GetTodasAsConquistas()
        {
            return new List<ConquistaViewModel>
            {
                new ConquistaViewModel 
                {
                    Nome = "Primeiros Passos no Saber",
                    Descricao = "Completou sua primeira atividade.",
                    Icone = "bi-check-lg", 
                    Desbloqueada = false  
                },
                new ConquistaViewModel 
                {
                    Nome = "Explorador do Conhecimento",
                    Descricao = "Completou 5 atividades.",
                    Icone = "bi-bookmark-check",
                    Desbloqueada = false
                },
                new ConquistaViewModel 
                {
                    Nome = "Colecionador de Estrelas",
                    Descricao = "Alcançou 10 estrelas.",
                    Icone = "bi-star-fill",
                    Desbloqueada = false
                },
                new ConquistaViewModel 
                {
                    Nome = "Mente Matemática",
                    Descricao = "Completou uma atividade de Matemática.",
                    Icone = "bi-calculator",
                    Desbloqueada = false
                },
                new ConquistaViewModel 
                {
                    Nome = "Viajante do Tempo",
                    Descricao = "Completou uma atividade de História.",
                    Icone = "bi-bank",
                    Desbloqueada = false
                },
                new ConquistaViewModel 
                {
                    Nome = "Pequeno Cientista",
                    Descricao = "Completou uma atividade de Ciências.",
                    Icone = "bi-lightbulb",
                    Desbloqueada = false
                }
               
            };
        }
        
        
        public List<ConquistaViewModel> CheckAchievements(Crianca crianca, List<RespostaAtividade> respostas, List<Atividade> atividadesDaSessao)
        {
 
            var todasAsConquistas = GetTodasAsConquistas();


            if (respostas.Count >= 1)
            {
                var conquista = todasAsConquistas.FirstOrDefault(c => c.Nome == "Primeiros Passos no Saber");
                if (conquista != null) conquista.Desbloqueada = true;
            }

          
            if (respostas.Count >= 5)
            {
                var conquista = todasAsConquistas.FirstOrDefault(c => c.Nome == "Explorador do Conhecimento");
                if (conquista != null) conquista.Desbloqueada = true;
            }

          
            if (crianca.Estrelas >= 10)
            {
                var conquista = todasAsConquistas.FirstOrDefault(c => c.Nome == "Colecionador de Estrelas");
                if (conquista != null) conquista.Desbloqueada = true;
            }

         
            var categoriasCompletas = respostas
                .Join(atividadesDaSessao, 
                      resposta => resposta.AtividadeId,
                      atividade => atividade.Id,
                      (resposta, atividade) => atividade.Categoria)
                .Distinct()
                .ToList();

            if (categoriasCompletas.Contains("Matemática"))
            {
                var conquista = todasAsConquistas.FirstOrDefault(c => c.Nome == "Mente Matemática");
                if (conquista != null) conquista.Desbloqueada = true;
            }
            if (categoriasCompletas.Contains("História"))
            {
                var conquista = todasAsConquistas.FirstOrDefault(c => c.Nome == "Viajante do Tempo");
                if (conquista != null) conquista.Desbloqueada = true;
            }
            if (categoriasCompletas.Contains("Ciências"))
            {
                var conquista = todasAsConquistas.FirstOrDefault(c => c.Nome == "Pequeno Cientista");
                if (conquista != null) conquista.Desbloqueada = true;
            }

         
            return todasAsConquistas;
        }
    }
}