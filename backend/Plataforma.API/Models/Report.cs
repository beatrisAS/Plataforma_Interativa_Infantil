namespace Plataforma.API.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int ChildId { get; set; }
        public required Child Child { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.Now;
        public required string ReportData { get; set; }
    }
}