using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.ViewModels
{
    public class CriarAtividadeViewModel
    {
        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Titulo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public string Categoria { get; set; } = string.Empty;

        public string? FaixaEtaria { get; set; }

        [MinLength(1, ErrorMessage = "A atividade deve ter pelo menos uma questão.")]
        public List<QuestaoViewModel> Questoes { get; set; } = new();
    }

    public class QuestaoViewModel
    {
        [Required(ErrorMessage = "O texto da pergunta é obrigatório.")]
        public string Pergunta { get; set; } = string.Empty;
        
        public string? Explicacao { get; set; }

        [MinLength(4), MaxLength(4)]
        public List<AlternativaViewModel> Alternativas { get; set; } = new() 
        { 
            new(), new(), new(), new() 
        };

        [Required(ErrorMessage = "Selecione a resposta correta.")]
        public int? AlternativaCorretaIndex { get; set; }
    }

    public class AlternativaViewModel
    {
        [Required(ErrorMessage = "O texto da alternativa é obrigatório.")]
        public string Texto { get; set; } = string.Empty;
        public bool Correta { get; set; }
    }
}