using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

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

        // --- CORREÇÃO ---
        // As propriedades foram marcadas como nullable (string?) para permitir
        // que o banco de dados retorne valores nulos sem causar um erro de conversão.
        [Column("descricao")]
        public string? Descricao { get; set; }

        [Column("faixa_etaria")]
        public string? FaixaEtaria { get; set; }
        
        // Esta coleção é usada pelas atividades dinâmicas e pelas criadas por professores.
        public ICollection<Questao> Questoes { get; set; } = new List<Questao>();
    }
}

