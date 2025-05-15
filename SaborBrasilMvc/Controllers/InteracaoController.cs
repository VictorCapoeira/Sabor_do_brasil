using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SaborBrasildbContext;
using DtosBrasil;
using Microsoft.EntityFrameworkCore;
using SaborBrasilMvc.Models;
using SaborBrasilTabelas.Models;

namespace SaborBrasilMvc.Controllers
{
    public class InteracaoController : Controller
    {
        private readonly SaborBrasilContext _context;

        public InteracaoController(SaborBrasilContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Like([FromBody] InteracaoDto dto)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return Unauthorized();

            // Remove deslike se existir
            var deslike = await _context.Interacoes
                .FirstOrDefaultAsync(i => i.PublicacaoId == dto.PublicacaoId && i.UsuarioId == usuarioId && i.Tipo == "deslike");
            if (deslike != null) _context.Interacoes.Remove(deslike);

            // Adiciona like se não existir
            var like = await _context.Interacoes
                .FirstOrDefaultAsync(i => i.PublicacaoId == dto.PublicacaoId && i.UsuarioId == usuarioId && i.Tipo == "like");
            if (like == null)
            {
                _context.Interacoes.Add(new Interacao
                {
                    PublicacaoId = dto.PublicacaoId,
                    UsuarioId = usuarioId.Value,
                    Tipo = "like"
                });
            }
            else
            {
                _context.Interacoes.Remove(like); // Descurtir se já curtiu
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Deslike([FromBody] InteracaoDto dto)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return Unauthorized();

            // Remove like se existir
            var like = await _context.Interacoes
                .FirstOrDefaultAsync(i => i.PublicacaoId == dto.PublicacaoId && i.UsuarioId == usuarioId && i.Tipo == "like");
            if (like != null) _context.Interacoes.Remove(like);

            // Adiciona deslike se não existir
            var deslike = await _context.Interacoes
                .FirstOrDefaultAsync(i => i.PublicacaoId == dto.PublicacaoId && i.UsuarioId == usuarioId && i.Tipo == "deslike");
            if (deslike == null)
            {
                _context.Interacoes.Add(new Interacao
                {
                    PublicacaoId = dto.PublicacaoId,
                    UsuarioId = usuarioId.Value,
                    Tipo = "deslike"
                });
            }
            else
            {
                _context.Interacoes.Remove(deslike); // Remove deslike se já descurtiu
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }

    public class InteracaoDto
    {
        public int PublicacaoId { get; set; }
    }
}