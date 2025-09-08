namespace Plataforma.API.Models
{
    public class Notificacao
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public required Usuario Usuario { get; set; }

        public required string Mensagem { get; set; }
        public bool Lida { get; set; } = false;
        public DateTime CriadoEm { get; set; } = DateTime.Now;
    }
}
