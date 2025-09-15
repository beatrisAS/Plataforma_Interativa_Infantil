namespace backend.Models;

public class Comentario 
{
    public int Id { get; set; }
    public string Texto { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    public int AtividadeId { get; set; }
    public string Status { get; set; } = "Pending"; 
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow; 
    
    
    public Usuario Usuario { get; set; } = null!;
    public Atividade Atividade { get; set; } = null!;
}