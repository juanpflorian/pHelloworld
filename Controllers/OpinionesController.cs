using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pHelloworld.Models;
using System.Security.Claims;

namespace pHelloworld.Controllers
{
    [Authorize]
    public class OpinionesController : Controller
    {
        private readonly AppDbContext _context;

        public OpinionesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Opinion opinion)
        {
            Console.WriteLine("⚡ POST Opinión llegó al servidor");

            Console.WriteLine("➡️ Datos recibidos:");
            Console.WriteLine($"   • IdGuia: {opinion.IdGuia}");
            Console.WriteLine($"   • Calificación: {opinion.Calificacion}");
            Console.WriteLine($"   • Comentario: {opinion.Comentario}");

            // Verificar que el usuario al que se dirige la opinión existe
            var receptorExiste = await _context.Usuarios
                .AnyAsync(u => u.id_usuario == opinion.IdGuia);

            if (!receptorExiste)
            {
                Console.WriteLine("❌ El usuario receptor no existe");
                return NotFound("El usuario receptor no existe");
            }

            // Obtener ID del usuario autenticado
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine($"🔐 ID autenticado (turista): {idClaim}");

            if (string.IsNullOrEmpty(idClaim))
            {
                Console.WriteLine("❌ No se encontró el ID del usuario autenticado");
                return Forbid();
            }

            // Prevenir opiniones a uno mismo
            var idActual = int.Parse(idClaim);
            if (idActual == opinion.IdGuia)
            {
                Console.WriteLine("❌ No puedes dejar una opinión sobre ti mismo");
                return BadRequest("No puedes dejar una opinión sobre ti mismo");
            }

            // Asignar campos requeridos
            opinion.IdTurista = idActual;
            opinion.Fecha = DateTime.UtcNow;

            // Validar el modelo
            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ Modelo inválido:");
                foreach (var kvp in ModelState)
                {
                    foreach (var error in kvp.Value.Errors)
                    {
                        Console.WriteLine($"   🔸 {kvp.Key}: {error.ErrorMessage}");
                    }
                }

                return BadRequest("Modelo inválido");
            }

            // Agregar opinión a la base de datos
            _context.Opiniones.Add(opinion);

            // Crear notificación para el receptor
            var notificacion = new Notificacion
            {
                IdUsuario = opinion.IdGuia,
                Titulo = "Nueva Opinión Recibida",
                Mensaje = "Has recibido una nueva opinión.",
                Fecha = DateTime.UtcNow,
                Leido = false
            };

            _context.Notificacion.Add(notificacion);

            // Guardar todo
            await _context.SaveChangesAsync();
            Console.WriteLine("✅ Opinión y notificación guardadas correctamente");

            return Ok("Opinión guardada");
        }
    }
}
