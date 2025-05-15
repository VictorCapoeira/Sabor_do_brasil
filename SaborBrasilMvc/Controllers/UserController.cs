using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SaborBrasilMvc.Models;
using SaborBrasildbContext;
using DtosBrasil;
using Microsoft.EntityFrameworkCore;

namespace UserDados
{
    public class UserController : Controller
    {
        private readonly SaborBrasilContext _context;

        // Injeção de dependência do DbContext
        public UserController(SaborBrasilContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetUserInfo()
        {
            var user = await _context.Usuarios
                .Include(e => e.Publicacoes)
                    .ThenInclude(p => p.Interacoes)
                .Select(e => new UserInfoDto
                {
                    Nome = e.Nome,
                    Foto = e.Foto,
                    TotalLikes = e.Publicacoes.SelectMany(p => p.Interacoes).Count(i => i.Tipo == "like"),
                    TotalDeslikes = e.Publicacoes.SelectMany(p => p.Interacoes).Count(i => i.Tipo == "deslike")
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                user = new UserInfoDto
                {
                    Nome = "User não encontrado",
                    Foto = "caminho_da_logo_padrao.png",
                    TotalLikes = 0,
                    TotalDeslikes = 0
                };
            }

            // Especifique o caminho da view Index da Home
            return View("~/Views/Home/Index.cshtml", user);
        }
    }
}
