using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("criancas")]
public class Crianca 
{
    [Column("id_crianca")]
    public int Id { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = string.Empty;
    
    [Column("data_nascimento")]
    public DateTime DataNascimento { get; set; }
    
    [Column("genero")]
    public string Genero { get; set; } = string.Empty;
    
    [Column("id_responsavel")]
    public int? IdResponsavel { get; set; }

    [ForeignKey("IdResponsavel")]
    public Usuario? Responsavel { get; set; }

    public int Estrelas { get; set; }
}
