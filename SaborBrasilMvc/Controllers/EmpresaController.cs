using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SaborBrasilMvc.Models;
using SaborBrasildbContext;
using DtosBrasil;
using Microsoft.EntityFrameworkCore;

namespace EmpresaDados
{
    public class EmpresaController : Controller
    {
        private readonly SaborBrasilContext _context;

        // Injeção de dependência do DbContext
        public EmpresaController(SaborBrasilContext context)
        {
            _context = context;
        }

        // Ação para retornar as informações da empresa
        public async Task<IActionResult> GetEmpresaInfo()
        {
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

            if (empresa == null)
            {
                empresa = new EmpresaInfoDto
                {
                    Nome = "Empresa não encontrada",
                    Logo = "caminho_da_logo_padrao.png",
                    TotalLikes = 0,
                    TotalDeslikes = 0
                };
            }

            // Especifique o caminho da view Index da Home
            return View("~/Views/Home/Index.cshtml", empresa);
        }
    }
}
