using backend.Models;
using System.Collections.Generic;

namespace backend.ViewModels
{
    public class CriancaDashboardViewModel
    {
        public required Crianca Crianca { get; set; }
        public List<Atividade> Atividades { get; set; } = new();
        public List<string> Conquistas { get; set; } = new();
        public List<string> CategoriasUnicas { get; set; } = new();
    }
}