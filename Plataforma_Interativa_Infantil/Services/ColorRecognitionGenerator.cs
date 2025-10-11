using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.Services
{
    public class ColorRecognitionGenerator : IActivityGenerator
    {
        public string Categoria => "Cores";
        private readonly Random _random = new();
        private readonly Dictionary<string, string> cores = new()
        {
            { "Vermelho", "#FF0000" },
            { "Azul", "#0000FF" },
            { "Verde", "#008000" },
            { "Amarelo", "#FFFF00" },
            { "Laranja", "#FFA500" },
            { "Roxo", "#800080" },
            { "Rosa", "#FFC0CB" },
            { "Preto", "#000000" }
        };

        public Atividade GenerateActivity(int id)
        {
            var atividade = new Atividade
            {
                Id = id,
                Titulo = "Que Cor é Essa?",
                Descricao = "Selecione o nome correto da cor exibida.",
                Categoria = Categoria,
                FaixaEtaria = "3-5 anos"
            };

           
            var coresUsadas = cores.Keys.OrderBy(c => _random.Next()).Take(5).ToList();

            for (int i = 0; i < 5; i++)
            {
                string corCorreta = coresUsadas[i];
                var questao = new Questao
                {
                    Id = i + 1,
                    Ordem = i + 1,
                  
                    Pergunta = $"Qual o nome desta cor? ({cores[corCorreta]})", 
                    Tipo = "multipla"
                };

                
                var alternativas = new List<Alternativa>
                {
                    new() { Id = 1, Texto = corCorreta, Correta = true }
                };

              
                var coresIncorretas = cores.Keys.Where(c => c != corCorreta).OrderBy(c => _random.Next()).Take(3);
                foreach (var cor in coresIncorretas)
                {
                    alternativas.Add(new Alternativa { Id = alternativas.Count + 1, Texto = cor, Correta = false });
                }
                
              
                questao.Alternativas = alternativas.OrderBy(a => _random.Next()).ToList();
                atividade.Questoes.Add(questao);
            }
            
            return atividade;
        }
    }
}
