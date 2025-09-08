namespace Plataforma.API.Models
{
    public class Option
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        // Relacionamento com Activity
        public int ActivityId { get; set; }
        public required Activity Activity { get; set; }
    }
}
