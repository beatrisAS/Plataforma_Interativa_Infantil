namespace backend.Models;
public class Atividade {
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string FaixaEtaria { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
}
