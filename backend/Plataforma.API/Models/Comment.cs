namespace Plataforma.API.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int ChildId { get; set; }
        public required Child Child { get; set; }

        public int SpecialistId { get; set; }
        public required User Specialist { get; set; }

        public required string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
