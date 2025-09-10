namespace backend.Models;
public class RespostaAtividade {
    public int Id { get; set; }
    public int CriancaId { get; set; }
    public int AtividadeId { get; set; }
    public int Desempenho { get; set; }
    public System.DateTime DataRealizacao { get; set; }
}
