using backend.Models;
using System.Collections.Generic;

namespace backend.ViewModels
{
    public class ProfessorDashboardViewModel
    {
        public List<CriancaProgressoViewModel> Alunos { get; set; } = new();
        public List<Atividade> AtividadesPublicadas { get; set; } = new();
        public int TotalAlunos { get; set; }
        public int TotalAtividades { get; set; }
        public int MediaGeral { get; set; }
        public int RespostasRecebidas { get; set; }
    }
}