using Microsoft.AspNetCore.Mvc;
using SaborBrasildbContext;
using DtosBrasil;
using Microsoft.EntityFrameworkCore;

namespace UserDados
{
    public class AccountController : Controller
    {
        private readonly SaborBrasilContext _context;

        public AccountController(SaborBrasilContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == login.Email && u.Senha == login.Senha);

            if (usuario == null)
                return Unauthorized(new { message = "Usuário ou senha incorretos." });

            // Salva o ID do usuário na sessão
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);

            return Ok(new { message = "Login realizado com sucesso!", usuarioId = usuario.Id });
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Aqui você pode limpar a sessão/cookie se estiver usando autenticação
            // Exemplo para sessão:
            HttpContext.Session.Clear();
            return Ok();
        }
    }
}