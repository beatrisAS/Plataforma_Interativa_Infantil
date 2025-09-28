using backend.Models;
using System.Collections.Generic;
using System.Linq;

namespace backend.Services
{
    public class AchievementService
    {
        // Este método verifica todo o progresso da criança e retorna uma lista de conquistas desbloqueadas.
        public List<string> CheckAchievements(Crianca crianca, List<RespostaAtividade> respostas, List<Atividade> atividadesDaSessao)
        {
            var conquistas = new List<string>();

            // Conquista: Completar a primeira atividade
            if (respostas.Count >= 1)
            {
                conquistas.Add("Primeiros Passos no Saber");
            }

            // Conquista: Completar 5 ou mais atividades
            if (respostas.Count >= 5)
            {
                conquistas.Add("Explorador do Conhecimento");
            }

            // Conquista: Acumular 10 ou mais estrelas
            if (crianca.Estrelas >= 10)
            {
                conquistas.Add("Colecionador de Estrelas");
            }

            // Conquistas por categoria: verifica as atividades completas
            var categoriasCompletas = respostas
                .Join(atividadesDaSessao, // Une as respostas com as atividades da sessão para obter a categoria
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
