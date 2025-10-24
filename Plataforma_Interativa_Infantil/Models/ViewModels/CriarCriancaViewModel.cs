using System;
using System.ComponentModel.DataAnnotations;

namespace backend.ViewModels
{
    public class CriarCriancaViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string NomeCompleto { get; set; } = string.Empty; 

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty; 

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty; 

        [Required(ErrorMessage = "A data de nascimento é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
        

        public string? Genero { get; set; } 
    }
}