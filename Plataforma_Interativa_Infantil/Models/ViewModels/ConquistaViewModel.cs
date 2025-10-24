
namespace backend.ViewModels
{
    public class ConquistaViewModel
    {
        public required string Nome { get; set; }
        public required string Descricao { get; set; }
        public required string Icone { get; set; }
        public bool Desbloqueada { get; set; }
    }
}