using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using backend.ViewModels; 
namespace backend.Models;

[Table("respostas_atividades")]
public class RespostaAtividade 
{
    [Column("id_resposta")]
    public int Id { get; set; }

    [Column("id_crianca")]
    public int CriancaId { get; set; }

    [Column("id_atividade")]
    public int AtividadeId { get; set; }

    [Column("desempenho")]
    public int Desempenho { get; set; }

    [Column("data_realizacao")]
    public DateTime DataRealizacao { get; set; }

    public Crianca Crianca { get; set; } = null!;
    public Atividade Atividade { get; set; } = null!;
    
        [NotMapped]
        public List<HistoricoTentativaViewModel> Historico { get; set; } = new();
}
