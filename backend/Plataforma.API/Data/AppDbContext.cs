using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Plataforma.API.Models;
using Activity = Plataforma.API.Models.Activity;

namespace Plataforma.API.Data
{
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Activity> Activities => Set<Activity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }
   
    public DbSet<Response> Responses { get; set; }

        private string GetDebuggerDisplay()
        {
#pragma warning disable CS8603 // Possible null reference return.

            return ToString();
#pragma warning restore CS8603 // Possible null reference return.

        }
    }
}
