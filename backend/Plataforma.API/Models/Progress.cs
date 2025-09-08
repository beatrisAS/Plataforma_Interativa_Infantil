namespace Plataforma.API.Models
{
    public class Progress
    {
        public int Id { get; set; }
        public int ChildId { get; set; }
        public required Child Child { get; set; }

        public int ActivityId { get; set; }
        public required Activity Activity { get; set; }

        public DateTime? CompletedAt { get; set; }
        public decimal? Score { get; set; }
        public required string Notes { get; set; }
    }
}