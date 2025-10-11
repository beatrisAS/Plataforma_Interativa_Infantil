using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using backend.Models; 

namespace backend.Models
{
    [Table("atividades")]
    public class Atividade
    {
        [Column("id_atividade")]
        public int Id { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [Column("categoria")]
        public string Categoria { get; set; } = string.Empty;

        [Column("descricao")]
        public string? Descricao { get; set; }

        [Column("faixa_etaria")]
        public string? FaixaEtaria { get; set; }
        
        public ICollection<Questao> Questoes { get; set; } = new List<Questao>();

        [Column("id_professor")] 
        public int? ProfessorId { get; set; }

      
        [ForeignKey("ProfessorId")]
        public Usuario? Professor { get; set; }
    }
}