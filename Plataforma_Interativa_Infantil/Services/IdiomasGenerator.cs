using backend.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace backend.Services
{
    public class IdiomasGenerator : IActivityGenerator
    {
        public string Categoria => "Idiomas";
        private readonly Random _random = new();
        public Atividade GenerateActivity(int id)
        {
            var questions = new List<Tuple<string, string, string[]>>
            {
                Tuple.Create("Como se diz 'cachorro' em inglês?", "Dog", new[] { "Cat", "Bird", "Fish" }),
                Tuple.Create("Qual o significado da expressão em inglês 'Hello, how are you?'", "Olá, como você está?", new[] { "Qual o seu nome?", "Onde você mora?", "Tchau, até logo" }),
                Tuple.Create("Como se diz 'gracias' em português?", "Obrigado(a)", new[] { "Por favor", "De nada", "Com licença" }),
                Tuple.Create("O que significa 'Bonjour' em francês?", "Bom dia", new[] { "Boa noite", "Boa tarde", "Até logo" }),
                Tuple.Create("Qual número vem depois de 'nine' em inglês?", "Ten", new[] { "Eight", "Seven", "Eleven" })
            };
            return CreateActivity(id, "Volta ao Mundo", questions);
        }
        private Atividade CreateActivity(int id, string title, List<Tuple<string, string, string[]>> questions)
        {
            var atividade = new Atividade { Id = id, Titulo = title, Descricao = $"Aprenda novas palavras em {Categoria}!", Categoria = Categoria, FaixaEtaria = "7-12 anos" };
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