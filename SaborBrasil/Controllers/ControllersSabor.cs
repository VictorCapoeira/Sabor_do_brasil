using Microsoft.AspNetCore.Mvc;
using SaborBrasilTabelas;
using SaborBrasilContext;
using Microsoft.EntityFrameworkCore;

namespace SaborDeMinas.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly AppDbContext _context;

        public EmpresaController(AppDbContext context)
        {
            _context = context;
        }

        // Método para trazer os dados da empresa (nome, logo e endereço)
        public async Task<IActionResult> Detalhes()
        {
            // Aqui buscamos a empresa com seu endereço, usando Include para trazer as entidades relacionadas
            var empresa = await _context.Empresas
                .Include(e => e.Endereco)  // Incluindo os dados do endereço
                .FirstOrDefaultAsync();    // Selecionando a primeira empresa (ou pode usar Where para especificar)

            // Verifica se a empresa foi encontrada
            if (empresa == null)
            {
                return NotFound(); // Retorna erro caso não encontre nenhuma empresa
            }

            // Retorna a empresa encontrada para a view
            return View(empresa);
        }
    }
}
