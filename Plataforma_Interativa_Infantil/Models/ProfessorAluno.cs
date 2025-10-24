using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Table("professor_alunos")]
    // Define a chave primária composta
    [PrimaryKey(nameof(ProfessorId), nameof(CriancaId))] 
    public class ProfessorAluno
    {
        [Column("id_professor")]
        public int ProfessorId { get; set; }

        [ForeignKey("ProfessorId")]
        public Usuario? Professor { get; set; }

        [Column("id_crianca")]
        public int CriancaId { get; set; }

        [ForeignKey("CriancaId")]
        public Crianca? Crianca { get; set; }
    }
}