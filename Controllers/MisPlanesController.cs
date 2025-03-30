using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pHelloworld.Data;
using pHelloworld.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using pHelloworld.Filtros;
using pHelloworld.Controllers; // Necesario para _LayoutController


namespace pHelloworld.Controllers
{
    [ServiceFilter(typeof(CargarNotificacionesFiltro))]
    [ServiceFilter(typeof(CargarMensajesFiltro))]
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
            // 🔔 Cargar mensajes para el layout
            var layout = new _LayoutController(_context);
            layout.ControllerContext = this.ControllerContext;
            await layout.CargarMensajesEnViewBag();

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
            var reserva = await _context.Reservas
                .Include(r => r.Plan)
                .Include(r => r.Turista)
                .FirstOrDefaultAsync(r => r.IdReserva == dto.id);

            if (reserva == null)
                return NotFound();

            reserva.Estado = dto.estado;
            await _context.SaveChangesAsync();

            // 🔔 Crear notificación para el turista si el guía acepta o cancela la reserva
            string titulo = "";
            string mensaje = "";

            if (dto.estado == "Confirmada")
            {
                titulo = "Reserva aceptada";
                mensaje = $"Tu reserva para el plan '{reserva.Plan.NombrePlan}' ha sido aceptada por el guía.";
            }
            else if (dto.estado == "Cancelada")
            {
                titulo = "Reserva cancelada por el guía";
                mensaje = $"El guía ha cancelado tu reserva para el plan '{reserva.Plan.NombrePlan}'.";
            }

            if (!string.IsNullOrEmpty(titulo))
            {
                var notificacion = new Notificacion
                {
                    IdUsuario = reserva.IdTurista,
                    Titulo = titulo,
                    Mensaje = mensaje,
                    Fecha = DateTime.UtcNow,
                    IdReserva = reserva.IdReserva
                };

                _context.Notificacion.Add(notificacion);
                await _context.SaveChangesAsync();
            }

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
