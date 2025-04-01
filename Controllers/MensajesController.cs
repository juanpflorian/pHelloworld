using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pHelloworld.Models;
using pHelloworld.Filtros;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace pHelloworld.Controllers
{
    [ServiceFilter(typeof(CargarNotificacionesFiltro))]
    [ServiceFilter(typeof(CargarMensajesFiltro))]
    public class MensajesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MensajesController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Mensajes/Chat/5
        public async Task<IActionResult> Chat(int id)
        {
            var idUsuarioActual = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var mensajes = await _context.Mensaje
                .Where(m => (m.IdEmisor == idUsuarioActual && m.IdReceptor == id) ||
                            (m.IdEmisor == id && m.IdReceptor == idUsuarioActual))
                .OrderBy(m => m.FechaEnvio)
                .ToListAsync();

            var receptor = await _context.Usuarios.FirstOrDefaultAsync(u => u.id_usuario == id);
            var yo = await _context.Usuarios.FirstOrDefaultAsync(u => u.id_usuario == idUsuarioActual);

            ViewBag.ReceptorId = id;
            ViewBag.ReceptorNombre = receptor?.Nombre ?? "Desconocido";
            ViewBag.ReceptorFoto = receptor?.foto_perfil ?? "~/img/fotoperfil.png";
            ViewBag.MiFoto = yo?.foto_perfil ?? "~/img/fotoperfil.png";

            return View(mensajes);
        }

        [HttpPost]
        public async Task<IActionResult> Enviar(int receptorId, string? contenido, IFormFile imagen)
        {
            var idUsuarioActual = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            string? rutaImagen = null;

            Console.WriteLine($"📤 Imagen recibida: {imagen?.FileName} - Tamaño: {imagen?.Length}");

            // Subida de imagen si hay
            if (imagen != null && imagen.Length > 0)
            {
                try
                {
                    string carpetaDestino = Path.Combine(_webHostEnvironment.WebRootPath, "img", "chat");
                    Console.WriteLine($"🛠 Ruta física: {carpetaDestino}");

                    if (!Directory.Exists(carpetaDestino))
                    {
                        Console.WriteLine("📁 Carpeta no existe, se crea.");
                        Directory.CreateDirectory(carpetaDestino);
                    }

                    string nombreArchivo = $"{Guid.NewGuid()}_{Path.GetFileName(imagen.FileName)}";
                    string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivo);
                    Console.WriteLine($"💾 Guardando imagen en: {rutaCompleta}");

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await imagen.CopyToAsync(stream);
                    }

                    rutaImagen = $"/img/chat/{nombreArchivo}";
                    Console.WriteLine($"✅ Imagen guardada correctamente: {rutaImagen}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error al guardar la imagen: {ex.Message}");
                    TempData["Error"] = "Ocurrió un error al guardar la imagen.";
                    return RedirectToAction("Chat", new { id = receptorId });
                }
            }

            // Validar que haya contenido o imagen
            if (string.IsNullOrWhiteSpace(contenido) && string.IsNullOrEmpty(rutaImagen))
            {
                TempData["Error"] = "Debes enviar un mensaje o una imagen.";
                return RedirectToAction("Chat", new { id = receptorId });
            }

            var mensaje = new Mensaje
            {
                IdEmisor = idUsuarioActual,
                IdReceptor = receptorId,
                Contenido = string.IsNullOrWhiteSpace(contenido) ? null : contenido,
                ImagenRuta = rutaImagen,
                FechaEnvio = DateTime.UtcNow,
                Leido = false
            };

            _context.Mensaje.Add(mensaje);
            await _context.SaveChangesAsync();

            return RedirectToAction("Chat", new { id = receptorId });
        }

        public async Task CargarMensajesEnViewBag()
        {
            if (!User.Identity.IsAuthenticated) return;

            var idActual = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

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

            ViewBag.MensajesRecibidos = lista;
            ViewBag.CantidadMensajesNoLeidos = mensajes.Count(m => !m.Leido);
        }
    }
}
