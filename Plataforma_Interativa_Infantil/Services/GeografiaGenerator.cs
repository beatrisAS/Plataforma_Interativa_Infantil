using backend.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace backend.Services
{
    public class GeografiaGenerator : IActivityGenerator
    {
        public string Categoria => "Geografia";
        private readonly Random _random = new();
        public Atividade GenerateActivity(int id)
        {
            var questions = new List<Tuple<string, string, string[]>>
            {
                Tuple.Create("Qual é a capital do Brasil?", "Brasília", new[] { "Rio de Janeiro", "São Paulo", "Salvador" }),
                Tuple.Create("Qual é o maior oceano do mundo?", "Oceano Pacífico", new[] { "Oceano Atlântico", "Oceano Índico", "Oceano Ártico" }),
                Tuple.Create("Em qual continente fica o Egito?", "África", new[] { "Ásia", "Europa", "Oceania" }),
                Tuple.Create("Qual é o rio mais longo do mundo?", "Rio Nilo", new[] { "Rio Amazonas", "Rio Mississipi", "Rio Yangtzé" }),
                Tuple.Create("A Cordilheira dos Andes fica em qual continente?", "América do Sul", new[] { "América do Norte", "Europa", "Ásia" })
            };
            return CreateActivity(id, "Viajando pelo Mapa", questions);
        }
        private Atividade CreateActivity(int id, string title, List<Tuple<string, string, string[]>> questions)
        {
            var atividade = new Atividade { Id = id, Titulo = title, Descricao = $"Conheça o mundo com a {Categoria}!", Categoria = Categoria, FaixaEtaria = "7-12 anos" };
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
