using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using pHelloworld.Models;
using pHelloworld.Filtros;

namespace pHelloworld.Controllers
{
    [ServiceFilter(typeof(CargarNotificacionesFiltro))]
    [ServiceFilter(typeof(CargarMensajesFiltro))]
    public class GuiasController : Controller
    {
        private readonly AppDbContext _context;

        public GuiasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> guias()
        {
            var guias = await _context.Usuarios
                .Where(u => u.Tipo_Usuario == "Guía")
                .ToListAsync();

            // 🔁 Cambio aquí: hacerlo secuencialmente
            var guiasConOpiniones = new List<(Usuario guia, double promedio)>();

            foreach (var g in guias)
            {
                var promedio = await _context.Opiniones
                    .Where(o => o.IdGuia == g.id_usuario)
                    .Select(o => (double?)o.Calificacion)
                    .AverageAsync() ?? 0.0;

                guiasConOpiniones.Add((g, promedio));
            }

            ViewBag.GuiasConCalificacion = guiasConOpiniones;

            return View();
        }

        [ActionName("Perfil")] // <- Cambiamos la ruta para que sea /Guias/Perfil/{id}
        public async Task<IActionResult> PerfilGuia(int id)
        {
            var guia = await _context.Usuarios.FindAsync(id);
            if (guia == null) return NotFound();

            var opiniones = await _context.Opiniones
                .Include(o => o.Turista)
                .Where(o => o.IdGuia == id)
                .OrderByDescending(o => o.Fecha)
                .ToListAsync();

            var promedio = opiniones.Any() ? opiniones.Average(o => o.Calificacion) : 0;

            ViewBag.Opiniones = opiniones;
            ViewBag.PromedioCalificacion = promedio;

            return View("~/Views/perfil/perfilPublico.cshtml", guia);
        }
    }
}
