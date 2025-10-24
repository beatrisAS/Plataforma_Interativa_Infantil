using System.ComponentModel.DataAnnotations;

namespace backend.ViewModels
{
    public class ProfileEditViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;

      
        public string? NovaSenha { get; set; }
    }
}