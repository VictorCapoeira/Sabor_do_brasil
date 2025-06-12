using Microsoft.AspNetCore.Mvc;
using SaborBrasildbContext;
using SaborBrasilTabelas.Models;
using Microsoft.EntityFrameworkCore;
using DtosBrasil;

namespace SaborBrasilMvc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComentarioController : ControllerBase
    {
        private readonly SaborBrasilContext _context;
        public ComentarioController(SaborBrasilContext context) { _context = context; }

        public class ComentarioDto
        {
            public int Id { get; set; } // Corrige erro de ausência de Id
            public int PublicacaoId { get; set; }
            public string? Texto { get; set; }
        }

        [HttpPost("Adicionar")]
        public async Task<IActionResult> Adicionar([FromBody] ComentarioDto dto)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null) return Unauthorized();

            if (dto.PublicacaoId == 0 || string.IsNullOrWhiteSpace(dto.Texto))
                return BadRequest("Dados inválidos");

            var comentario = new Comentario
            {
                Texto = dto.Texto ?? string.Empty,
                PublicacaoId = dto.PublicacaoId,
                UsuarioId = usuarioId.Value,
                CreatedAt = DateTime.Now,
                Date = DateTime.Now.Date // <-- Adicione esta linha!
            };
            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();

            var usuario = await _context.Usuarios.FindAsync(usuarioId.Value);
            return Ok(new {
                texto = comentario.Texto,
                usuarioNome = usuario?.Nome ?? "",
                fotoUsuario = usuario?.Foto ?? "",
                comentarioId = comentario.Id,
                usuarioId = usuario?.Id ?? 0
            });
        }

        [HttpGet("PorPublicacao/{publicacaoId}")]
        public async Task<IActionResult> PorPublicacao(int publicacaoId)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var comentarios = await _context.Comentarios
                .Where(c => c.PublicacaoId == publicacaoId)
                .OrderByDescending(c => c.CreatedAt)
                .Select(c => new {
                    comentarioId = c.Id,
                    texto = c.Texto,
                    usuarioNome = c.Usuario != null ? c.Usuario.Nome : "",
                    fotoUsuario = c.Usuario != null ? c.Usuario.Foto : "",
                    usuarioId = c.UsuarioId,
                    podeEditar = usuarioId != null && c.UsuarioId == usuarioId
                })
                .ToListAsync();
            return Ok(comentarios);
        }

        [HttpPost("Editar")]
        public async Task<IActionResult> Editar([FromBody] ComentarioDto dto)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var comentario = await _context.Comentarios.FindAsync(dto.Id);
            if (comentario == null)
            {
                Console.WriteLine($"Comentário não encontrado para Id={dto.Id}");
                return NotFound("Comentário não encontrado");
            }
            if (comentario.UsuarioId != usuarioId)
            {
                Console.WriteLine($"Usuário não autorizado. UsuarioId={usuarioId}, Comentario.UsuarioId={comentario.UsuarioId}");
                return Forbid();
            }

            comentario.Texto = dto.Texto ?? string.Empty;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("Excluir")]
        public async Task<IActionResult> Excluir([FromBody] ComentarioDto dto)
        {
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var comentario = await _context.Comentarios.FindAsync(dto.Id);
            if (comentario == null)
            {
                Console.WriteLine($"Comentário não encontrado para Id={dto.Id}");
                return NotFound("Comentário não encontrado");
            }
            if (comentario.UsuarioId != usuarioId)
            {
                Console.WriteLine($"Usuário não autorizado. UsuarioId={usuarioId}, Comentario.UsuarioId={comentario.UsuarioId}");
                return Forbid();
            }

            _context.Comentarios.Remove(comentario);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}