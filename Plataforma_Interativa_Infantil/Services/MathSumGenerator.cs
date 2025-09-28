using backend.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace backend.Services
{
    public class MatematicaGenerator : IActivityGenerator
    {
        public string Categoria => "Matemática";
        private readonly Random _random = new();

        public Atividade GenerateActivity(int id)
        {
            var questions = new List<Tuple<string, string, string[]>>
            {
                Tuple.Create("Quanto é 12 x 8?", "96", new[] { "88", "104", "92" }),
                Tuple.Create("Se um trem viaja a 80 km/h, que distância ele percorre em 3 horas?", "240 km", new[] { "210 km", "160 km", "320 km" }),
                Tuple.Create("Qual é o resultado de 5² (cinco ao quadrado)?", "25", new[] { "10", "50", "125" }),
                Tuple.Create("Um livro custa R$ 35. Se você pagar com uma nota de R$ 50, qual será o troco?", "R$ 15", new[] { "R$ 5", "R$ 10", "R$ 25" }),
                Tuple.Create("Qual é o próximo número na sequência: 7, 14, 21, 28...?", "35", new[] { "32", "42", "36" })
            };
            
            return CreateActivity(id, "Desafios Matemáticos", questions);
        }

        private Atividade CreateActivity(int id, string title, List<Tuple<string, string, string[]>> questions)
        {
            var atividade = new Atividade
            {
                Id = id,
                Titulo = title,
                Descricao = $"Mostre suas habilidades em {Categoria}!",
                Categoria = Categoria,
                FaixaEtaria = "7-12 anos"
            };

            var selectedQuestions = questions.OrderBy(x => _random.Next()).Take(5).ToList();
            int questionId = 1;
            foreach (var q in selectedQuestions)
            {
                var alternativas = new List<Alternativa> { new() { Texto = q.Item2, Correta = true } };
                alternativas.AddRange(q.Item3.Select(text => new Alternativa { Texto = text, Correta = false }));

                atividade.Questoes.Add(new Questao
                {
                    Id = questionId++,
                    Pergunta = q.Item1,
                    Alternativas = alternativas.OrderBy(a => _random.Next()).ToList()
                });
            }
            return atividade;
        }
    }
}