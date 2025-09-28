using backend.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace backend.Services
{
    public class ArtesGenerator : IActivityGenerator
    {
        public string Categoria => "Artes";
        private readonly Random _random = new();
        public Atividade GenerateActivity(int id)
        {
            var questions = new List<Tuple<string, string, string[]>>
            {
                Tuple.Create("Quem pintou a Mona Lisa?", "Leonardo da Vinci", new[] { "Vincent van Gogh", "Pablo Picasso", "Michelangelo" }),
                Tuple.Create("Quais são as três cores primárias?", "Vermelho, azul e amarelo", new[] { "Verde, laranja e roxo", "Branco, preto e cinza", "Vermelho, verde и azul" }),
                Tuple.Create("Qual artista brasileiro é famoso por suas pinturas com bandeirinhas e temas de festas juninas?", "Alfredo Volpi", new[] { "Tarsila do Amaral", "Candido Portinari", "Romero Britto" }),
                Tuple.Create("A arte de dobrar papéis para criar figuras é chamada de...", "Origami", new[] { "Kirigami", "Mosaico", "Escultura" }),
                Tuple.Create("Que instrumento musical tem 88 teclas pretas e brancas?", "Piano", new[] { "Violão", "Flauta", "Bateria" })
            };
            return CreateActivity(id, "Oficina de Arte", questions);
        }
        private Atividade CreateActivity(int id, string title, List<Tuple<string, string, string[]>> questions)
        {
            var atividade = new Atividade { Id = id, Titulo = title, Descricao = $"Solte sua criatividade em {Categoria}!", Categoria = Categoria, FaixaEtaria = "7-12 anos" };
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
