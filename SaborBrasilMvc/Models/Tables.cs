using System.ComponentModel.DataAnnotations.Schema;
namespace SaborBrasilTabelas.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Apelido { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public string? Foto { get; set; }

        public List<Publicacao>? Publicacoes { get; set; }
        public List<Interacao>? Interacoes { get; set; }
        public List<Comentario>? Comentarios { get; set; }
    }

    public class Empresa{
        public int Id{ get; set; }
        public string Nome{ get; set; } = string.Empty;
        public string Logo{ get; set; } = string.Empty;
        public List<Publicacao>? Publicacoes { get; set; }
        public EmpresaEndereco? Endereco{ get; set; }
    }
    public class EmpresaEndereco{
        public int Id { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Rua { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;

        [Column("empresa_id")]
        public int EmpresaId { get; set; }
        public Empresa? Empresa { get; set; } 
    }
    public class Publicacao
        {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Imagem { get; set; }
        public string? Descricao { get; set; }
        public string Local { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        [Column("empresa_id")]
        public int EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public List<Interacao>? Interacoes { get; set; }
        public List<Comentario>? Comentarios { get; set; }
    }
    public class Interacao
    {
        public int Id { get; set; }

        public string Tipo { get; set; } = string.Empty; // "like" ou "deslike"
        [Column("publicacao_id")]
        public int PublicacaoId { get; set; }
        public Publicacao? Publicacao { get; set; }
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
    public class Comentario
    {
        public int Id { get; set; }
        public string Texto { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        [Column("publicacao_id")]
        public int PublicacaoId { get; set; }
        public Publicacao Publicacao { get; set; }
        public string Foto { get; set; }
    }
}