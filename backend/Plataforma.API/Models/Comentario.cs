namespace Plataforma.API.Models
{
    public class Comentario
    {
        public int Id { get; set; }
        public int CriancaId { get; set; }
        public required Crianca Crianca { get; set; }

        public int EspecialistaId { get; set; }
        public required Usuario Especialista { get; set; }

        public required string Texto { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.Now;
    }
}
