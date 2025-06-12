using Microsoft.EntityFrameworkCore;
using SaborBrasilTabelas.Models;

namespace SaborBrasildbContext
{
    public class SaborBrasilContext : DbContext
    {
        public SaborBrasilContext(DbContextOptions<SaborBrasilContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<EmpresaEndereco> EmpresasEnderecos { get; set; }
        public DbSet<Publicacao> Publicacoes { get; set; }
        public DbSet<Interacao> Interacoes { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
    }
}
