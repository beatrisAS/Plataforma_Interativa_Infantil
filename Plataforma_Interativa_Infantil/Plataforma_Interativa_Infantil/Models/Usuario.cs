using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models;

public class Usuario {
    [Key]
    [Column("id_usuario")]
    public int Id { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("perfil")]
    public string Perfil { get; set; } = "user";

    // Agora armazenamos a senha diretamente
    [Column("senha")]
    public string Senha { get; set; } = string.Empty;
}
