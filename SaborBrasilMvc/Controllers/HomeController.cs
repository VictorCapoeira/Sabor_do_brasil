using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SaborBrasildbContext;
using DtosBrasil;
using Microsoft.EntityFrameworkCore;
using SaborBrasilMvc.Models;


namespace SaborBrasilMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly SaborBrasilContext _context;

        public HomeController(SaborBrasilContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Busca empresa (ajuste para buscar a empresa correta se necessário)
            var empresa = await _context.Empresas
                .Include(e => e.Publicacoes)
                    .ThenInclude(p => p.Interacoes)
                .Select(e => new EmpresaInfoDto
                {
                    Nome = e.Nome,
                    Logo = e.Logo,
                    TotalLikes = e.Publicacoes.SelectMany(p => p.Interacoes).Count(i => i.Tipo == "like"),
                    TotalDeslikes = e.Publicacoes.SelectMany(p => p.Interacoes).Count(i => i.Tipo == "deslike")
                })
                .FirstOrDefaultAsync();

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");

            // Busca usuário logado (ajuste para pegar o usuário da sessão/cookie)
            UserInfoDto usuario = null;

            if (usuarioId.HasValue)
            {
                usuario = await _context.Usuarios
                    .Where(u => u.Id == usuarioId.Value)
                    .Select(u => new UserInfoDto
                    {
                        Nome = u.Nome,
                        Foto = u.Foto,
                        TotalLikes = u.Interacoes.Count(i => i.Tipo == "like"),
                        TotalDeslikes = u.Interacoes.Count(i => i.Tipo == "deslike")
                    })
                    .FirstOrDefaultAsync();
            }

            // Busca publicações (exemplo simplificado)
            var publicacoes = await _context.Publicacoes
                .Include(p => p.Interacoes)
                .Include(p => p.Comentarios)
                .Select(p => new PublicacaoDto
                {
                    Id = p.Id,
                    Titulo = p.Titulo,
                    Imagem = p.Imagem,
                    Descricao = p.Descricao,
                    Local = p.Local,
                    Likes = p.Interacoes.Count(i => i.Tipo == "like"),
                    Dislikes = p.Interacoes.Count(i => i.Tipo == "deslike"),
                    Comentarios = p.Comentarios.Count(),
                    UsuarioCurtiu = usuarioId.HasValue && p.Interacoes.Any(i => i.UsuarioId == usuarioId.Value && i.Tipo == "like"),
                    UsuarioDescurtiu = usuarioId.HasValue && p.Interacoes.Any(i => i.UsuarioId == usuarioId.Value && i.Tipo == "deslike")
                })
                .ToListAsync();

            // Busca comentários (exemplo simplificado)
            var comentarios = await _context.Comentarios
                .Select(c => new ComentarioDto
                {
                    Texto = c.Texto,
                    UsuarioNome = c.Usuario.Nome,
                    FotoUsuario = c.Usuario.Foto
                })
                .ToListAsync();

            var dto = new PaginaPrincipalDto
            {
                Empresa = empresa,
                Usuario = usuario,
                Publicacoes = publicacoes,
                Comentarios = comentarios
            };

            return View(dto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
