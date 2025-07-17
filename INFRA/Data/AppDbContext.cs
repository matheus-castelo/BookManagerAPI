using INFRA.Entidades;
using Microsoft.EntityFrameworkCore;

namespace INFRA.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<LivroAutor> LivrosAutores { get; set; }
        public DbSet<LivroGenero> LivrosGeneros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(eb =>
            {
                eb.HasKey(u => u.Id);
                eb.HasIndex(u => u.Email).IsUnique();

                eb.Property(u => u.SenhaHash)
                  .HasColumnType("BLOB")
                  .IsRequired();
                eb.Property(u => u.SenhaSalt)
                  .HasColumnType("BLOB")
                  .IsRequired();
            });

            modelBuilder.Entity<Livro>().HasKey(l => l.Id);

            modelBuilder.Entity<Autor>().HasKey(a => a.Id);

            modelBuilder.Entity<Genero>().HasKey(g => g.Id);

            modelBuilder.Entity<Avaliacao>(eb =>
            {
                eb.HasKey(a => a.Id);
                eb.HasIndex(a => new { a.UsuarioId, a.LivroId }).IsUnique();

                eb.HasOne(a => a.Usuario)
                  .WithMany(u => u.Avaliacoes)
                  .HasForeignKey(a => a.UsuarioId)
                  .OnDelete(DeleteBehavior.Cascade);

                eb.HasOne(a => a.Livro)
                  .WithMany(l => l.Avaliacoes)
                  .HasForeignKey(a => a.LivroId)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LivroAutor>(eb =>
            {
                eb.HasKey(la => new { la.LivroId, la.AutorId });

                eb.HasOne(la => la.Livro)
                  .WithMany(l => l.LivrosAutores)
                  .HasForeignKey(la => la.LivroId)
                  .OnDelete(DeleteBehavior.Cascade);

                eb.HasOne(la => la.Autor)
                  .WithMany(a => a.LivrosAutores)
                  .HasForeignKey(la => la.AutorId)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<LivroGenero>(eb =>
            {
                eb.HasKey(lg => new { lg.LivroId, lg.GeneroId });

                eb.HasOne(lg => lg.Livro)
                  .WithMany(l => l.LivrosGeneros)
                  .HasForeignKey(lg => lg.LivroId)
                  .OnDelete(DeleteBehavior.Cascade);

                eb.HasOne(lg => lg.Genero)
                  .WithMany(g => g.LivrosGeneros)
                  .HasForeignKey(lg => lg.GeneroId)
                  .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
