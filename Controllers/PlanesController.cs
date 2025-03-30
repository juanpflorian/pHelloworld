using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using pHelloworld.Data;

namespace pHelloworld.Controllers
{
    public class PlanesController : Controller
    {
        private readonly AppDbContext _context;

        public PlanesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Planes()
        {
            var idUsuario = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.IdUsuario = idUsuario;

            var planes = await _context.Planes.Include(p => p.Guia).ToListAsync();

            return View("~/Views/Gestion/planes.cshtml", planes);
        }
    }
}
