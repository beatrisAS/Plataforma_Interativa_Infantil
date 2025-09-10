namespace backend.Models;
using System;

public enum ComentarioStatus { Pending = 0, Approved = 1, Rejected = 2 }

public class Comentario {
    public int Id { get; set; }
    public int AtividadeId { get; set; }
    public int UsuarioId { get; set; }
    public string Texto { get; set; } = string.Empty;
    public ComentarioStatus Status { get; set; } = ComentarioStatus.Pending;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
