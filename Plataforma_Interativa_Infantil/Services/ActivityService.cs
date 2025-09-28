using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.Services
{
    // A INTERFACE (O CONTRATO PARA TODOS OS GERADORES)
    public interface IActivityGenerator
    {
        string Categoria { get; }
        Atividade GenerateActivity(int id);
    }

    // O SERVIÇO PRINCIPAL (O MAESTRO)
    public class ActivityService
    {
        private readonly List<IActivityGenerator> _generators;
        private readonly Random _random = new();

        public ActivityService()
        {
            // Registra todos os 7 geradores de atividades,
            // assumindo que cada um está em seu próprio arquivo.
            _generators = new List<IActivityGenerator>
            {
                new MatematicaGenerator(),
                new LiteraturaGenerator(),
                new CienciasGenerator(),
                new ArtesGenerator(),
                new IdiomasGenerator(),
                new GeografiaGenerator(),
                new HistoriaGenerator()
            };
        }

        // Este método garante uma atividade de cada categoria, sem repetições
        public List<Atividade> GetRandomActivities(int count = 7)
        {
            // Embaralha a lista de geradores para que a ordem mude a cada vez
            var shuffledGenerators = _generators.OrderBy(g => _random.Next()).ToList();
            
            var activities = new List<Atividade>();
            for (int i = 0; i < Math.Min(count, shuffledGenerators.Count); i++)
            {
                // Gera a atividade usando um ID único (i + 1)
                activities.Add(shuffledGenerators[i].GenerateActivity(i + 1));
            }
            
            return activities;
        }
    }
}
