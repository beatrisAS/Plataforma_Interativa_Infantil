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
        
        // Adicionada para permitir o cálculo da idade na tela do pai
        public DateTime DataNascimento { get; set; } 

        // Lista com todas as tentativas da criança
        public List<RespostaAtividade> Respostas { get; set; } = new();
        
        // Lista com apenas as atividades únicas que a criança já respondeu
        public List<Atividade> AtividadesUnicas { get; set; } = new();
    }
}