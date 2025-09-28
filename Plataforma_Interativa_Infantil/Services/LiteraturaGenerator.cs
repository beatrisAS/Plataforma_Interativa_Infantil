using backend.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace backend.Services
{
    public class LiteraturaGenerator : IActivityGenerator
    {
        public string Categoria => "Literatura";
        private readonly Random _random = new();
        public Atividade GenerateActivity(int id)
        {
            var questions = new List<Tuple<string, string, string[]>>
            {
                Tuple.Create("Quem escreveu 'O Sítio do Picapau Amarelo'?", "Monteiro Lobato", new[] { "Ziraldo", "Mauricio de Sousa", "Ruth Rocha" }),
                Tuple.Create("No livro 'O Pequeno Príncipe', de qual planeta ele veio?", "Asteroide B-612", new[] { "Marte", "Júpiter", "Saturno" }),
                Tuple.Create("Qual personagem do folclore brasileiro tem uma perna só?", "Saci-Pererê", new[] { "Curupira", "Boitatá", "Mula sem Cabeça" }),
                Tuple.Create("Quem são os dois personagens principais das histórias de Mauricio de Sousa que têm um coelho de pelúcia famoso?", "Mônica e Cebolinha", new[] { "Cascão e Magali", "Chico Bento e Rosinha", "Penadinho e Zé Vampir" }),
                Tuple.Create("O que é um 'Haicai'?", "Um poema curto de origem japonesa", new[] { "Um tipo de romance de aventura", "Uma história em quadrinhos", "Uma peça de teatro" })
            };
            return CreateActivity(id, "Mundo da Leitura", questions);
        }
        private Atividade CreateActivity(int id, string title, List<Tuple<string, string, string[]>> questions)
        {
            var atividade = new Atividade { Id = id, Titulo = title, Descricao = $"Explore o universo da {Categoria}!", Categoria = Categoria, FaixaEtaria = "7-12 anos" };
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