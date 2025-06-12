namespace DtosBrasil
{
    public class PaginaPrincipalDto
    {
        public EmpresaInfoDto Empresa { get; set; }
        public UserInfoDto Usuario { get; set; }
        public List<PublicacaoDto> Publicacoes { get; set; }
        public List<ComentarioDto> Comentarios { get; set; }
        // Adicione outros campos conforme necessário
    }

    public class EmpresaInfoDto
    {
        public string Nome { get; set; }
        public string Logo { get; set; }
        public int TotalLikes { get; set; }
        public int TotalDeslikes { get; set; }
    }

    public class UserInfoDto
    {
        public string Nome { get; set; }
        public string Foto { get; set; }
        public int TotalLikes { get; set; }
        public int TotalDeslikes { get; set; }
    }

    public class PublicacaoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Imagem { get; set; }
        public string Descricao { get; set; }
        public string Local { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Comentarios { get; set; }
        public bool UsuarioCurtiu { get; set; }
        public bool UsuarioDescurtiu { get; set; }
    }

    public class ComentarioDto
    {
        public string Texto { get; set; }
        public string UsuarioNome { get; set; }
        public string FotoUsuario { get; set; }
        // Outros campos...
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
