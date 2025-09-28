using backend.Models;
using System.Collections.Generic;

namespace backend.ViewModels
{
    // Usado para exibir o progresso de cada criança no dashboard do pai/professor
    public class CriancaProgressoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int Estrelas { get; set; }
        public List<RespostaAtividade> Respostas { get; set; } = new();
    }
}

