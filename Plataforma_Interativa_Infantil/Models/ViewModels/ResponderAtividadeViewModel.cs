using System.Collections.Generic;

namespace backend.ViewModels
{
    public class ResponderAtividadeViewModel
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public List<QuestaoViewModel> Questoes { get; set; } = new();
    }
}