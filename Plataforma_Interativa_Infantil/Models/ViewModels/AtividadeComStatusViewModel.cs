
using backend.Models;

namespace backend.ViewModels
{
    public class AtividadeComStatusViewModel
    {
        public required Atividade Atividade { get; set; }
        public bool Concluida { get; set; }
    }
}