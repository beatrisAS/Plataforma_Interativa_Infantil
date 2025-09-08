namespace Plataforma.API.Models
{
    public class Child
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime BirthDate { get; set; }

        // Relationship
        public int GuardianId { get; set; }
        public required User Guardian { get; set; }
    }
}