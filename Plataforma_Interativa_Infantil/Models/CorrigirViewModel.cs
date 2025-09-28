using System.ComponentModel.DataAnnotations;

namespace backend.ViewModels
{
    public class CorrigirViewModel
    {
        public int RespostaId { get; set; }
        public string NomeCrianca { get; set; } = string.Empty;
        public string TituloAtividade { get; set; } = string.Empty;
        public int Desempenho { get; set; }

        [Required(ErrorMessage = "A nota é obrigatória.")]
        [Range(0, 10, ErrorMessage = "A nota deve ser entre 0 e 10.")]
        public int Nota { get; set; }

        public string? Observacao { get; set; }
    }
}
