using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Crianca> Criancas { get; set; } = null!;
    public DbSet<Questao> Questoes { get; set; } = null!;
public DbSet<Alternativa> Alternativas { get; set; } = null!;
    public DbSet<Atividade> Atividades { get; set; } = null!;
    public DbSet<RespostaAtividade> RespostasAtividades { get; set; } = null!;
    public DbSet<Comentario> Comentarios { get; set; } = null!;

}