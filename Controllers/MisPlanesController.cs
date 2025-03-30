
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pHelloworld.Data;
using pHelloworld.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pHelloworld.Controllers
{
    [Authorize(Roles = "Guía")] // solo guías pueden acceder
    public class MisplanesController : Controller
    {
        private readonly AppDbContext _context;

        public MisplanesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> MisPlanes()
        {
            // Obtener id del usuario autenticado desde los claims
            var idGuia = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(idGuia))
            {
                return Unauthorized();
            }

            int id = int.Parse(idGuia);

            var planes = await _context.Planes
                .Where(p => p.IdGuia == id)
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();

            return View("~/Views/Gestion/MisPlanes.cshtml", planes);
        }

        [HttpGet]
        public async Task<IActionResult> ReservasPorPlan(int id)
        {
            var reservas = await _context.Reservas
                .Include(r => r.Turista)
                .Where(r => r.IdPlan == id)
                .Select(r => new
                {
                    id = r.IdReserva,
                    nombre = r.Turista.Nombre,
                    correo = r.Turista.Correo,
                    fecha = r.FechaProgramada.ToString("yyyy-MM-dd"),
                    estado = r.Estado
                })
                .ToListAsync();

            return Json(reservas);
        }

        public class ReservaEstadoDto
        {
            public int id { get; set; }
            public string estado { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstadoReserva([FromBody] ReservaEstadoDto dto)
        {
            var reserva = await _context.Reservas.FindAsync(dto.id);
            if (reserva == null)
                return NotFound();

            reserva.Estado = dto.estado;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Estado actualizado" });
        }

        [HttpPost]
        public async Task<IActionResult> CrearPlan([FromBody] Plan nuevoPlan)
        {
            var idGuia = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idGuia))
                return Unauthorized();

            nuevoPlan.IdGuia = int.Parse(idGuia);
            nuevoPlan.FechaCreacion = DateTime.Now;

            _context.Planes.Add(nuevoPlan);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Plan creado con éxito" });
        }





    }
}
