using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

[Table("questoes")]
public class Questao
{
    [Column("id_questao")]
    public int Id { get; set; }

    [Column("id_atividade")]
    public int AtividadeId { get; set; }
    public Atividade? Atividade { get; set; }

    [Column("pergunta")]
    public string Pergunta { get; set; } = string.Empty;

    [Column("tipo")]
    public string Tipo { get; set; } = "multipla";

    public List<Alternativa> Alternativas { get; set; } = new();
    
       public int Ordem { get; set; } 
}

