using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.ViewModels
{
    public class QuestaoViewModel
    {
        [Required(ErrorMessage = "O texto da pergunta é obrigatório.")]
        public string Pergunta { get; set; } = string.Empty;
        
        public string? Explicacao { get; set; }

        [MinLength(4, ErrorMessage = "A questão deve ter 4 alternativas.")]
        [MaxLength(4)]
        public List<AlternativaViewModel> Alternativas { get; set; } = new() 
        { 
            // Esta linha está correta e vai funcionar
            // depois que você corrigir o AlternativaViewModel
            new(), new(), new(), new() 
        };

        [Required(ErrorMessage = "Selecione a resposta correta.")]
        public int? AlternativaCorretaIndex { get; set; }
    }
}