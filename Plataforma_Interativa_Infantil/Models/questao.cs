using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic; // Necessário para List<>

namespace backend.Models
{
    [Table("questoes")]
    public class Questao
    {
        [Column("id_questao")]
        public int Id { get; set; }

        [Column("id_atividade")] // Corrigido de "id_atividade" para "atividade_id" para bater com seu SQL
        public int AtividadeId { get; set; }
        public Atividade? Atividade { get; set; }

        [Column("pergunta")]
        public string Pergunta { get; set; } = string.Empty;
        
        // --- ADICIONE ESTAS DUAS LINHAS ---
        [Column("explicacao")] // Mapeia para a coluna do banco
        public string? Explicacao { get; set; }
        // ---------------------------------

        [Column("tipo")]
        public string Tipo { get; set; } = "multipla";

        public List<Alternativa> Alternativas { get; set; } = new();
        
        public int Ordem { get; set; } 
    }
}