using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("atividades")]
public class Atividade
{
    [Column("id_atividade")]
    public int Id { get; set; }

    [Column("titulo")]
    public string Titulo { get; set; } = string.Empty;

    [Column("descricao")]
    public string Descricao { get; set; } = string.Empty;

    [Column("faixa_etaria")]
    public string FaixaEtaria { get; set; } = string.Empty;

    [Column("categoria")]
    public string Categoria { get; set; } = string.Empty;
    
      public int Ordem { get; set; }

    // Relacionamentos
    public ICollection<Questao> Questoes { get; set; } = new List<Questao>();
}
