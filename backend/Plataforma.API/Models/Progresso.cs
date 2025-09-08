namespace Plataforma.API.Models
{
    public class Progresso
    {
        public int Id { get; set; }
        public int CriancaId { get; set; }
        public required Crianca Crianca { get; set; }

        public int AtividadeId { get; set; }
        public required Atividade Atividade { get; set; }

        public DateTime? DataConclusao { get; set; }
        public decimal? Pontuacao { get; set; }
        public required string Observacoes { get; set; }
    }
}
