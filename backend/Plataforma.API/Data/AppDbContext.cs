using Microsoft.EntityFrameworkCore;
using Plataforma.API.Models;

namespace Plataforma.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Crianca> Criancas { get; set; }
        public DbSet<Atividade> Atividades { get; set; }
        public DbSet<Progresso> Progresso { get; set; }
        public DbSet<Relatorio> Relatorios { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }
    }
}
