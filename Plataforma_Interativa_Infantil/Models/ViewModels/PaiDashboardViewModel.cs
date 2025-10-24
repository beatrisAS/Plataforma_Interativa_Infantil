using backend.Models;
using System.Collections.Generic;

namespace backend.ViewModels
{
 
    public class PaiDashboardViewModel
    {
      
        public CriancaDetalheViewModel? CriancaDetalhe { get; set; }
        

        public bool TemCriancasVinculadas => CriancaDetalhe != null;
    }


    public class CriancaDetalheViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Idade { get; set; }
        
        public List<AtividadeRealizadaViewModel> AtividadesRealizadas { get; set; } = new List<AtividadeRealizadaViewModel>();
        public List<AtividadePendenteViewModel> AtividadesPendentes { get; set; } = new List<AtividadePendenteViewModel>();
        
      
        public List<ChartDataViewModel> MediaPorMateria { get; set; } = new List<ChartDataViewModel>();
    }

    public class AtividadeRealizadaViewModel
    {
        public int AtividadeId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int Nota { get; set; } 
        public int Acertos { get; set; } 
        public int TotalQuestoes { get; set; } 
        public int Tentativas { get; set; }
    }

    public class AtividadePendenteViewModel
    {
        public int AtividadeId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
    }

    public class ChartDataViewModel
    {
        public string Label { get; set; } = string.Empty; 
        public int Value { get; set; }
    }
}
