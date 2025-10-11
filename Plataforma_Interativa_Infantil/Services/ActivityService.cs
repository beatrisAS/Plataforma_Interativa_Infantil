using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.Services
{

    public interface IActivityGenerator
    {
        string Categoria { get; }
        Atividade GenerateActivity(int id);
    }


    public class ActivityService
    {
        private readonly List<IActivityGenerator> _generators;
        private readonly Random _random = new();

        public ActivityService()
        {
       
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

       
        public List<Atividade> GetRandomActivities(int count = 7)
        {
          
            var shuffledGenerators = _generators.OrderBy(g => _random.Next()).ToList();
            
            var activities = new List<Atividade>();
            for (int i = 0; i < Math.Min(count, shuffledGenerators.Count); i++)
            {
               
                activities.Add(shuffledGenerators[i].GenerateActivity(i + 1));
            }
            
            return activities;
        }
    }
}
