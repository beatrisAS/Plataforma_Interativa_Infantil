namespace backend.Models;

public class RespostaAtividade 
{
    public int Id { get; set; }
    public int CriancaId { get; set; }
    public int AtividadeId { get; set; }
    public int Desempenho { get; set; }
    public DateTime DataRealizacao { get; set; }
    public Crianca Crianca { get; set; } = null!;
    public Atividade Atividade { get; set; } = null!;
}
