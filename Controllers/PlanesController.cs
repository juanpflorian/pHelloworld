using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using pHelloworld.Data;
using pHelloworld.Controllers; // 👈 Importante para usar _LayoutController
using pHelloworld.Filtros;

namespace pHelloworld.Controllers
{
    [ServiceFilter(typeof(CargarNotificacionesFiltro))]
    [ServiceFilter(typeof(CargarMensajesFiltro))]
    public class PlanesController : Controller
    {
        private readonly AppDbContext _context;

        public PlanesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Planes()
        {
            // 🔔 Cargar mensajes para el layout
            var layout = new _LayoutController(_context);
            layout.ControllerContext = this.ControllerContext;
            await layout.CargarMensajesEnViewBag();

            var idUsuario = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.IdUsuario = idUsuario;

            var planes = await _context.Planes.Include(p => p.Guia).ToListAsync();

            return View("~/Views/Gestion/planes.cshtml", planes);
        }
    }
}
