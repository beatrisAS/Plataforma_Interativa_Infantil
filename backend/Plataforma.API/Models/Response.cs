namespace Plataforma.API.Models
{
    public class Response
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public string Answer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // relacionamento
        public required Activity Activity { get; set; }
    }
}
