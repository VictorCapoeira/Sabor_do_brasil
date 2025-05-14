namespace DtosBrasil
{
    public class EmpresaInfoDto
        {
            public string Nome { get; set; }
            public string Logo { get; set; }
            public int TotalLikes { get; set; }
            public int TotalDeslikes { get; set; }
        }
// DTOs/LoginDto.cs
    public class LoginDto
        {
            public string Email { get; set; }
            public string Senha { get; set; }
        }

}
