using Microsoft.EntityFrameworkCore;
using Plataforma.API.Models;

namespace Plataforma.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Progress> Progress { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Garantir que os nomes das tabelas sejam consistentes com o banco
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Child>().ToTable("Children");
            modelBuilder.Entity<Activity>().ToTable("Activities");
            modelBuilder.Entity<Progress>().ToTable("Progress");
            modelBuilder.Entity<Report>().ToTable("Reports");
            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<Notification>().ToTable("Notifications");

            base.OnModelCreating(modelBuilder);
        }
    }
}
