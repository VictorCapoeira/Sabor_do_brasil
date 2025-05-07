using Microsoft.EntityFrameworkCore;
using SaborBrasilTabelas;

namespace SaborBrasilContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<EmpresaEndereco> EmpresasEndereco { get; set; }
        public DbSet<Publicacao> Publicacoes { get; set; }
        public DbSet<Interacao> Interacoes { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tabela usuarios
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Apelido)
                .IsUnique();

            // Tabela empresas_endereco
            modelBuilder.Entity<EmpresaEndereco>()
                .HasOne(e => e.Empresa)
                .WithOne(e => e.Endereco)
                .HasForeignKey<EmpresaEndereco>(e => e.EmpresaId);

            // Tabela publicacoes
            modelBuilder.Entity<Publicacao>()
                .HasOne(p => p.Empresa)
                .WithMany(e => e.Publicacoes)
                .HasForeignKey(p => p.EmpresaId);

            // Tabela interacoes
            modelBuilder.Entity<Interacao>()
                .Property(i => i.Tipo)
                .HasConversion<string>(); // enum-like string

            modelBuilder.Entity<Interacao>()
                .HasOne(i => i.Usuario)
                .WithMany(u => u.Interacoes)
                .HasForeignKey(i => i.UsuarioId);

            modelBuilder.Entity<Interacao>()
                .HasOne(i => i.Publicacao)
                .WithMany(p => p.Interacoes)
                .HasForeignKey(i => i.PublicacaoId);

            // Tabela comentarios
            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Comentarios)
                .HasForeignKey(c => c.UsuarioId);

            modelBuilder.Entity<Comentario>()
                .HasOne(c => c.Publicacao)
                .WithMany(p => p.Comentarios)
                .HasForeignKey(c => c.PublicacaoId);
        }
    }
}
