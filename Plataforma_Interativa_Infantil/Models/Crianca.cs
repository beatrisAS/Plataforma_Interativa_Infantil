namespace backend.Models;
public class Crianca {
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public System.DateTime DataNascimento { get; set; }
    public string Genero { get; set; } = string.Empty;
    public int? IdResponsavel { get; set; }
}
