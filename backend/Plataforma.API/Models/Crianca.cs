namespace Plataforma.API.Models
{
    public class Crianca
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public DateTime DataNascimento { get; set; }

        // Relacionamento
        public int ResponsavelId { get; set; }
        public required Usuario Responsavel { get; set; }
    }
}
