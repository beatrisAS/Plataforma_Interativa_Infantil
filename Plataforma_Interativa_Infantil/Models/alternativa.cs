using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("alternativas")]
public class Alternativa {
    [Column("id_alternativa")]
    public int Id { get; set; }

   [Column("questao_id")]
public int QuestaoId { get; set; }
public Questao? Questao { get; set; }


    [Column("texto")]
    public string Texto { get; set; } = string.Empty;

    [Column("correta")]
    public bool Correta { get; set; }
}