using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pHelloworld.Data;
using pHelloworld.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using pHelloworld.Filtros;
using pHelloworld.Controllers; // Para usar _LayoutController

namespace pHelloworld.Controllers { 


    [ServiceFilter(typeof(CargarMensajesFiltro))]
public class ReservasController : Controller
    {
        private readonly AppDbContext _context;

        public ReservasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> MisReservas()
        {
            // 🔔 Cargar mensajes para el layout
            var layout = new _LayoutController(_context);
            layout.ControllerContext = this.ControllerContext;
            await layout.CargarMensajesEnViewBag();

            var idUsuarioClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(idUsuarioClaim))
                return Unauthorized("Debes iniciar sesión para ver tus reservas.");

            int idUsuario = int.Parse(idUsuarioClaim);

            var reservas = await _context.Reservas
                .Include(r => r.Guia)
                .Include(r => r.Plan)
                .Where(r => r.IdTurista == idUsuario && r.Estado != "Cancelada")
                .ToListAsync();

            return View("~/Views/Gestion/Reservas.cshtml", reservas);
        }

        [HttpPost]
        public async Task<IActionResult> Cancelar([FromBody] int idReserva)
        {
            Console.WriteLine($"Intentando cancelar la reserva con ID: {idReserva}");

            var reserva = await _context.Reservas.FindAsync(idReserva);
            if (reserva == null)
            {
                Console.WriteLine("Reserva no encontrada en la base de datos.");
                return Json(new { success = false, message = "La reserva no existe." });
            }

            if (reserva.Estado != "Pendiente")
            {
                Console.WriteLine("La reserva no está en estado Pendiente.");
                return Json(new { success = false, message = "Solo se pueden cancelar reservas pendientes." });
            }

            reserva.Estado = "Cancelada";
            await _context.SaveChangesAsync();

            Console.WriteLine("Reserva cancelada con éxito.");
            return Json(new { success = true, message = "Reserva cancelada con éxito." });
        }

        [HttpPost]
        public async Task<IActionResult> Reservar([FromBody] Reserva nuevaReserva)
        {
            try
            {
                Console.WriteLine("Intentando crear una nueva reserva...");

                if (nuevaReserva == null || nuevaReserva.IdPlan <= 0)
                {
                    Console.WriteLine("Datos de reserva inválidos.");
                    return Json(new { success = false, message = "Datos de reserva inválidos." });
                }

                var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(usuarioIdClaim))
                {
                    Console.WriteLine("El usuario no ha iniciado sesión.");
                    return Json(new { success = false, message = "Debes iniciar sesión para reservar un plan." });
                }

                int usuarioId = int.Parse(usuarioIdClaim);

                // Validación: no permitir fechas pasadas
                if (nuevaReserva.FechaProgramada.Date < DateTime.UtcNow.Date)
                {
                    Console.WriteLine("La fecha programada es anterior a hoy.");
                    return Json(new { success = false, message = "No puedes reservar una fecha anterior a la de hoy ! :)" });
                }

                var plan = await _context.Planes
                    .Include(p => p.Guia)
                    .FirstOrDefaultAsync(p => p.IdPlan == nuevaReserva.IdPlan);

                if (plan == null)
                {
                    Console.WriteLine("El plan no existe.");
                    return Json(new { success = false, message = "El plan seleccionado no existe." });
                }

                if (!plan.IdGuia.HasValue || plan.IdGuia <= 0)
                {
                    Console.WriteLine("El plan no tiene un guía asignado.");
                    return Json(new { success = false, message = "El plan no tiene un guía asignado o el ID es inválido." });
                }

                var reservaExistente = await _context.Reservas
                    .Where(r => r.IdTurista == usuarioId && r.IdPlan == nuevaReserva.IdPlan)
                    .OrderByDescending(r => r.FechaReserva)
                    .FirstOrDefaultAsync();

                if (reservaExistente != null && reservaExistente.Estado != "Cancelada")
                {
                    Console.WriteLine("El usuario ya tiene una reserva activa para este plan.");
                    return Json(new { success = false, message = "Ya tienes una reserva activa para este plan." });
                }

                var reserva = new Reserva
                {
                    IdTurista = usuarioId,
                    IdGuia = plan.IdGuia.Value,
                    IdPlan = plan.IdPlan,
                    FechaReserva = DateTime.UtcNow,
                    FechaProgramada = nuevaReserva.FechaProgramada,
                    Descripcion = plan.Descripcion,
                    Duracion = plan.Duracion,
                    Precio = plan.Precio.Value,
                    Estado = "Pendiente"
                };

                _context.Reservas.Add(reserva);
                await _context.SaveChangesAsync();

                string nombreGuia = plan.Guia != null ? plan.Guia.Nombre : "El guía";
                Console.WriteLine($"Reserva creada exitosamente. {nombreGuia} se contactará con usted.");
                return Json(new { success = true, message = $"Reserva creada con éxito. {nombreGuia} se contactará con usted pronto." });
            }
            catch (DbUpdateException dbEx)
            {
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                Console.WriteLine($"Error en la base de datos: {innerMessage}");
                return Json(new { success = false, message = $"Error en la base de datos: {innerMessage}" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado: {ex.Message}");
                return Json(new { success = false, message = $"Error inesperado: {ex.Message}" });
            }
        }
    }
}
