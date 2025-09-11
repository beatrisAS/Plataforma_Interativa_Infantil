using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Crianca 
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    
    [Column("data_nascimento")]
    public DateTime DataNascimento { get; set; }
    
    public string Genero { get; set; } = string.Empty;
    
    [Column("id_responsavel")]
    public int IdResponsavel { get; set; }
    
    // Propriedade de navegação
    public Usuario Responsavel { get; set; } = null!;
}