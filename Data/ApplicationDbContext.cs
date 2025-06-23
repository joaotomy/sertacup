using Microsoft.EntityFrameworkCore;
using SertaCup_site.Models;

namespace SertaCup_site.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Player> Player { get; set; } 

        public DbSet<Team> Team { get; set; }

        public DbSet<Game> Game { get; set; }
        public DbSet<Grupo> Grupos { get; set; }

        public DbSet<Classificacao> Classificacoes { get; set; }
        public DbSet<Marcador> Marcador { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}