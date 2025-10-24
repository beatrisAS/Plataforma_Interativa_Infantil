using System.ComponentModel.DataAnnotations;
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

    [Column("id_usuario")] 
    public int UsuarioId { get; set; } 

    [ForeignKey("UsuarioId")] 
    public Usuario Usuario { get; set; } = null!;

 
    [Column("codigo_vinculo")]
    [StringLength(10)] 
    public string CodigoDeVinculo { get; set; } = string.Empty;
}