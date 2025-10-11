using backend.Models;
using System.Collections.Generic;
using System.Linq;

namespace backend.Services
{
    public class AchievementService
    {
      
        public List<string> CheckAchievements(Crianca crianca, List<RespostaAtividade> respostas, List<Atividade> atividadesDaSessao)
        {
            var conquistas = new List<string>();

          
            if (respostas.Count >= 1)
            {
                conquistas.Add("Primeiros Passos no Saber");
            }

           
            if (respostas.Count >= 5)
            {
                conquistas.Add("Explorador do Conhecimento");
            }

            
            if (crianca.Estrelas >= 10)
            {
                conquistas.Add("Colecionador de Estrelas");
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
                conquistas.Add("Mente Matemática");
            }
            if (categoriasCompletas.Contains("História"))
            {
                conquistas.Add("Viajante do Tempo");
            }
            if (categoriasCompletas.Contains("Ciências"))
            {
                conquistas.Add("Pequeno Cientista");
            }

            return conquistas;
        }
    }
}
