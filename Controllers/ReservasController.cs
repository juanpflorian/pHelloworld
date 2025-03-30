using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pHelloworld.Data;
using pHelloworld.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using pHelloworld.Filtros;
using pHelloworld.Controllers;

namespace pHelloworld.Controllers
{
    [ServiceFilter(typeof(CargarNotificacionesFiltro))]
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
            var reserva = await _context.Reservas
                .Include(r => r.Plan)
                .FirstOrDefaultAsync(r => r.IdReserva == idReserva);

            if (reserva == null)
                return Json(new { success = false, message = "La reserva no existe." });

            if (reserva.Estado != "Pendiente")
                return Json(new { success = false, message = "Solo se pueden cancelar reservas pendientes." });

            reserva.Estado = "Cancelada";
            await _context.SaveChangesAsync();

            // ✅ Cargar el turista para incluir su nombre en la notificación
            var turista = await _context.Usuarios.FindAsync(reserva.IdTurista);

            // 🔔 Notificación al guía por cancelación
            var notificacion = new Notificacion
            {
                IdUsuario = reserva.IdGuia,
                Titulo = "Reserva cancelada",
                Mensaje = $"{turista?.Nombre ?? "Un turista"} ha cancelado la reserva del plan: {reserva.Plan.NombrePlan}",
                Fecha = DateTime.UtcNow,
                IdReserva = reserva.IdReserva
            };

            _context.Notificacion.Add(notificacion);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Reserva cancelada con éxito." });
        }


        [HttpPost]
        public async Task<IActionResult> Reservar([FromBody] Reserva nuevaReserva)
        {
            try
            {
                if (nuevaReserva == null || nuevaReserva.IdPlan <= 0)
                    return Json(new { success = false, message = "Datos de reserva inválidos." });

                var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(usuarioIdClaim))
                    return Json(new { success = false, message = "Debes iniciar sesión para reservar un plan." });

                int usuarioId = int.Parse(usuarioIdClaim);

                if (nuevaReserva.FechaProgramada.Date < DateTime.UtcNow.Date)
                    return Json(new { success = false, message = "No puedes reservar una fecha anterior a hoy." });

                var plan = await _context.Planes.Include(p => p.Guia).FirstOrDefaultAsync(p => p.IdPlan == nuevaReserva.IdPlan);
                if (plan == null || !plan.IdGuia.HasValue)
                    return Json(new { success = false, message = "El plan no existe o no tiene guía asignado." });

                var reservaExistente = await _context.Reservas
                    .Where(r => r.IdTurista == usuarioId && r.IdPlan == nuevaReserva.IdPlan)
                    .OrderByDescending(r => r.FechaReserva)
                    .FirstOrDefaultAsync();

                if (reservaExistente != null && reservaExistente.Estado != "Cancelada")
                    return Json(new { success = false, message = "Ya tienes una reserva activa para este plan." });

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

                // 🔔 Notificación al guía
                var turista = await _context.Usuarios.FindAsync(usuarioId);

                var notificacion = new Notificacion
                {
                    IdUsuario = plan.IdGuia.Value,
                    Titulo = "Nueva reserva creada",
                    Mensaje = $"{turista?.Nombre ?? "Un turista"} ha reservado tu plan: {plan.NombrePlan} para el {reserva.FechaProgramada:yyyy-MM-dd}.",
                    Fecha = DateTime.UtcNow,
                    IdReserva = reserva.IdReserva
                };


                _context.Notificacion.Add(notificacion);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Reserva creada con éxito. El guía se contactará contigo pronto." });
            }
            catch (DbUpdateException dbEx)
            {
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return Json(new { success = false, message = $"Error en la base de datos: {innerMessage}" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error inesperado: {ex.Message}" });
            }
        }
    }
}
