using System;

namespace backend.ViewModels
{
    public class HistoricoTentativaViewModel
    {
        public DateTime Data { get; set; }
        public int Pontuacao { get; set; } 
        public string TempoGasto { get; set; } = string.Empty; 
    }
}