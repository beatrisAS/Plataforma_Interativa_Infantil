using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.ViewModels
{
    // Este arquivo deve conter APENAS a classe CriarAtividadeViewModel
    public class CriarAtividadeViewModel
    {
        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Titulo { get; set; } = string.Empty;

        public string? Descricao { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        public string Categoria { get; set; } = string.Empty;

        public string? FaixaEtaria { get; set; }

        [MinLength(1, ErrorMessage = "A atividade deve ter pelo menos uma questão.")]
        // Ele vai usar as classes QuestaoViewModel e AlternativaViewModel 
        // dos outros arquivos que você já criou.
        public List<QuestaoViewModel> Questoes { get; set; } = new();
    }
}