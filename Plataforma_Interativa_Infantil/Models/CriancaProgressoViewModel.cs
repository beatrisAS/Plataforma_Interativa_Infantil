using backend.Models;
using System;
using System.Collections.Generic;

namespace backend.ViewModels
{
    public class CriancaProgressoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Estrelas { get; set; }
        
       
        public DateTime DataNascimento { get; set; } 

              public List<RespostaAtividade> Respostas { get; set; } = new();
        
    
        public List<Atividade> AtividadesUnicas { get; set; } = new();
    }
}