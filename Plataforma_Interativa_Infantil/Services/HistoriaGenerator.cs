using backend.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace backend.Services
{
    public class HistoriaGenerator : IActivityGenerator
    {
        public string Categoria => "História";
        private readonly Random _random = new();
        public Atividade GenerateActivity(int id)
        {
            var questions = new List<Tuple<string, string, string[]>>
            {
                Tuple.Create("Quem proclamou a Independência do Brasil?", "Dom Pedro I", new[] { "Dom Pedro II", "Tiradentes", "Marechal Deodoro" }),
                Tuple.Create("Em que ano os portugueses chegaram ao Brasil?", "1500", new[] { "1822", "1889", "1900" }),
                Tuple.Create("As pirâmides do Egito foram construídas para serem...", "Túmulos para os faraós", new[] { "Palácios para morar", "Templos de observação das estrelas", "Fortalezas de guerra" }),
                Tuple.Create("Qual foi a primeira capital do Brasil?", "Salvador", new[] { "Rio de Janeiro", "Brasília", "São Paulo" }),
                Tuple.Create("O Muro de Berlim dividia qual país em dois?", "Alemanha", new[] { "França", "Rússia", "Itália" })
            };
            return CreateActivity(id, "Viagem no Tempo", questions);
        }
        private Atividade CreateActivity(int id, string title, List<Tuple<string, string, string[]>> questions)
        {
            var atividade = new Atividade { Id = id, Titulo = title, Descricao = $"Aprenda sobre o passado com a {Categoria}!", Categoria = Categoria, FaixaEtaria = "7-12 anos" };
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