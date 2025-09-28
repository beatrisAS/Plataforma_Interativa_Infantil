using backend.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace backend.Services
{
    public class CienciasGenerator : IActivityGenerator
    {
        public string Categoria => "Ciências";
        private readonly Random _random = new();
        public Atividade GenerateActivity(int id)
        {
            var questions = new List<Tuple<string, string, string[]>>
            {
                Tuple.Create("Qual é o maior planeta do nosso sistema solar?", "Júpiter", new[] { "Saturno", "Terra", "Netuno" }),
                Tuple.Create("Qual processo as plantas usam para converter luz solar em alimento?", "Fotossíntese", new[] { "Respiração", "Digestão", "Evaporação" }),
                Tuple.Create("Quantos ossos um ser humano adulto possui em seu corpo?", "206", new[] { "188", "230", "300" }),
                Tuple.Create("Qual gás é essencial para a nossa respiração?", "Oxigênio", new[] { "Nitrogênio", "Gás Carbônico", "Hélio" }),
                Tuple.Create("O que causa as marés nos oceanos?", "A gravidade da Lua", new[] { "O vento", "O aquecimento do Sol", "Os terremotos" })
            };
            return CreateActivity(id, "Explorando a Ciência", questions);
        }
        private Atividade CreateActivity(int id, string title, List<Tuple<string, string, string[]>> questions)
        {
            var atividade = new Atividade { Id = id, Titulo = title, Descricao = $"Descubra os segredos da {Categoria}!", Categoria = Categoria, FaixaEtaria = "7-12 anos" };
            var selectedQuestions = questions.OrderBy(x => _random.Next()).Take(5).ToList();
            int questionId = 1;
            foreach (var q in selectedQuestions)
            {
                var alternativas = new List<Alternativa> { new() { Texto = q.Item2, Correta = true } };
                alternativas.AddRange(q.Item3.Select(text => new Alternativa { Texto = text, Correta = false }));
                atividade.Questoes.Add(new Questao { Id = questionId++, Pergunta = q.Item1, Alternativas = alternativas.OrderBy(a => _random.Next()).ToList() });
            }
            return atividade;
        }
    }
}
