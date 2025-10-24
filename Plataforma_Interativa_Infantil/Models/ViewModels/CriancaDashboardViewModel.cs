using backend.Models;
using System.Collections.Generic;

namespace backend.ViewModels
{
    public class CriancaDashboardViewModel
    {
   
        public required Crianca Crianca { get; set; }

 
        public List<AtividadeComStatusViewModel> Atividades { get; set; } = new(); 

        public List<ConquistaViewModel> Conquistas { get; set; } = new();

     
        public List<string> CategoriasUnicas { get; set; } = new();
    }
}