namespace Plataforma.API.Models
{
    public class Relatorio
    {
        public int Id { get; set; }
        public int CriancaId { get; set; }
        public required Crianca Crianca { get; set; }

        public DateTime GeradoEm { get; set; } = DateTime.Now;
        public required string DadosRelatorio { get; set; }
    }
}
