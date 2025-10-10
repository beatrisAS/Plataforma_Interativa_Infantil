using System;

namespace backend.ViewModels
{
    public class HistoricoTentativaViewModel
    {
        public DateTime Data { get; set; }
        public int Pontuacao { get; set; } // Ex: 80 para 80%
        public string TempoGasto { get; set; } = string.Empty; // Ex: "1m 15s"
    }
}