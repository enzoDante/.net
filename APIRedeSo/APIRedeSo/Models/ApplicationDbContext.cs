using Microsoft.EntityFrameworkCore;

namespace APIRedeSo.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Postagem> Postagem { get; set; }
        public DbSet<Fotos_postagem> Fotos_postagem { get; set; }
        public DbSet<Postagem_usuario_relacionado> Postagem_usuario_relacionado { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>().Property(u => u.Data_criacao).HasDefaultValueSql("CURRENT_TIMESTAMP");

            //base.OnModelCreating(modelBuilder);
            // Configura o relacionamento entre Fotos_postagem e Postagem, usando Postagem_id como FK
            modelBuilder.Entity<Fotos_postagem>()
                .HasOne(f => f.Postagem)
                .WithMany(p => p.Fotos)
                .HasForeignKey(f => f.Postagem_id)
                .OnDelete(DeleteBehavior.Cascade); // ou a restrição desejada

            modelBuilder.Entity<Postagem_usuario_relacionado>() //mesma coisa acima mas para Colaboradores
                .HasOne(f => f.Postagem)
                .WithMany(p => p.Colaboradores)
                .HasForeignKey(f => f.Postagem_id)
                .OnDelete(DeleteBehavior.Cascade);

        }

    }
}
