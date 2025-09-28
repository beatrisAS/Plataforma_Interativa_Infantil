using backend.Models;
using System.Collections.Generic;

namespace backend.ViewModels
{
    public class ProfessorDashboardViewModel
    {
        public List<CriancaProgressoViewModel> Alunos { get; set; } = new();
        public List<RespostaAtividade> RespostasPendentes { get; set; } = new();
    }
}
