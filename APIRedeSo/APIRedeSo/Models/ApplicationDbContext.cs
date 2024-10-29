using Microsoft.EntityFrameworkCore;

namespace APIRedeSo.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Postagem> Postagens { get; set; }
        public DbSet<Fotos_postagem> Fotos { get; set; }
        public DbSet<Postagem_usuario_relacionado> Colaboradores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().Property(u => u.Data_criacao).HasDefaultValueSql("CURRENT_TIMESTAMP");

            //base.OnModelCreating(modelBuilder);
        }

    }
}
