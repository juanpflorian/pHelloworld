using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using pHelloworld.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pHelloworld.Filtros
{
    public class CargarNotificacionesFiltro : IAsyncActionFilter
    {
        private readonly AppDbContext _context;

        public CargarNotificacionesFiltro(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as Controller;
            if (controller == null || !controller.User.Identity.IsAuthenticated)
            {
                await next();
                return;
            }

            var idUsuario = int.Parse(controller.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var notificaciones = await _context.Notificacion
                .Where(n => n.IdUsuario == idUsuario)
                .OrderByDescending(n => n.Fecha)
                .Take(20) // puedes ajustar cuántas mostrar
                .ToListAsync();

            var cantidadNoLeidas = notificaciones.Count(n => !n.Leido);

            controller.ViewBag.NotificacionesRecibidas = notificaciones;
            controller.ViewBag.CantidadNotificacionesNoLeidas = cantidadNoLeidas;

            await next();
        }
    }
}
