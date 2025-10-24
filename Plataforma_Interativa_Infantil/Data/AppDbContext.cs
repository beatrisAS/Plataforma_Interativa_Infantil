using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Crianca> Criancas { get; set; } = null!;
    public DbSet<ProfessorAluno> ProfessorAlunos { get; set; }
    public DbSet<Questao> Questoes { get; set; } = null!;
public DbSet<Alternativa> Alternativas { get; set; } = null!;
    public DbSet<Atividade> Atividades { get; set; } = null!;
    public DbSet<RespostaAtividade> RespostasAtividades { get; set; } = null!;
  

}