using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using pHelloworld.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pHelloworld.Filtros
{
    public class CargarMensajesFiltro : IAsyncActionFilter
    {
        private readonly AppDbContext _context;

        public CargarMensajesFiltro(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as Controller;
            var httpContext = context.HttpContext;

            if (controller != null && httpContext.User.Identity.IsAuthenticated)
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (int.TryParse(userIdClaim, out int idActual))
                {
                    var mensajes = await _context.Mensaje
                        .Where(m => m.IdReceptor == idActual)
                        .GroupBy(m => m.IdEmisor)
                        .Select(g => g.OrderByDescending(m => m.FechaEnvio).FirstOrDefault())
                        .ToListAsync();

                    var emisores = await _context.Usuarios
                        .Where(u => mensajes.Select(m => m.IdEmisor).Contains(u.id_usuario))
                        .ToListAsync();

                    var lista = mensajes
                        .Select(m => (Emisor: emisores.FirstOrDefault(u => u.id_usuario == m.IdEmisor), UltimoMensaje: m))
                        .ToList();

                    controller.ViewBag.MensajesRecibidos = lista;
                    controller.ViewBag.CantidadMensajesNoLeidos = mensajes.Count(m => !m.Leido);
                }
            }

            await next(); // Continuar con la ejecución del controlador
        }
    }
}
