using System.ComponentModel.DataAnnotations;

namespace backend.ViewModels
{
    public class AlternativaViewModel
    {
        // Use [Required] (um atributo) para validação
        [Required(ErrorMessage = "O texto da alternativa é obrigatório.")]
        public string Texto { get; set; } = string.Empty;
        
        public bool Correta { get; set; }
    }
}