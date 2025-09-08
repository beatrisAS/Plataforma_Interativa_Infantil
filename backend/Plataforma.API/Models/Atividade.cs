namespace Plataforma.API.Models
{
    public class Atividade
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public required string Descricao { get; set; }
        public required string Tipo { get; set; } // jogo, exercicio, quiz
    }
}
