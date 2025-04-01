using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pHelloworld.Data;
using pHelloworld.Models;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using pHelloworld.Filtros;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

using pHelloworld.Controllers; // Para acceder al _LayoutController

namespace pHelloworld.Controllers
{
    [ServiceFilter(typeof(CargarNotificacionesFiltro))]
    [ServiceFilter(typeof(CargarMensajesFiltro))]
    public class PerfilController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PerfilController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Perfil()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("IniciarSesion", "Login");

            // 🔔 Cargar mensajes para el layout
            var layout = new _LayoutController(_context);
            layout.ControllerContext = this.ControllerContext;
            await layout.CargarMensajesEnViewBag();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return RedirectToAction("IniciarSesion", "Login");

            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null)
                return RedirectToAction("IniciarSesion", "Login");

            var perfilViewModel = new Perfil
            {
                usuario = usuario.usuario ?? "Sin usuario",
                Nombre = usuario.Nombre ?? "Sin nombre",
                Correo = usuario.Correo ?? "Sin correo",
                Telefono = usuario.Telefono ?? "No registrado",
                Direccion = usuario.Direccion ?? "No especificada",
                TipoUsuario = usuario.Tipo_Usuario ?? "Turista",
                Idiomas = usuario.Idiomas ?? "No especificado",
                Nacionalidad = usuario.Nacionalidad ?? "No especificada",
                Especialidad = usuario.Especialidad ?? "No especificada",
                Experiencia = usuario.Experiencia ?? "No especificada",
                Disponibilidad = usuario.Disponibilidad ?? "No especificada",
                TarifaHora = usuario.TarifaHora ?? 0m,
                TarifaTour = usuario.TarifaTour ?? 0m,
                foto_perfil = string.IsNullOrEmpty(usuario.foto_perfil) ? "~/img/fotoperfil.png" : usuario.foto_perfil
            };

            var opiniones = await _context.Opiniones
                .Include(o => o.Turista)
                .Where(o => o.IdGuia == userId)
                .OrderByDescending(o => o.Fecha)
                .ToListAsync();
            ViewBag.PromedioCalificacion = opiniones.Any() ? opiniones.Average(o => o.Calificacion) : 0.0;

            ViewBag.Opiniones = opiniones;


            return View("~/Views/Perfil/Perfil.cshtml", perfilViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ModificarPerfil()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("IniciarSesion", "Login");

            // 🔔 Cargar mensajes para el layout
            var layout = new _LayoutController(_context);
            layout.ControllerContext = this.ControllerContext;
            await layout.CargarMensajesEnViewBag();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return RedirectToAction("IniciarSesion", "Login");

            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null)
                return RedirectToAction("IniciarSesion", "Login");

            var perfilViewModel = new Perfil
            {
                usuario = usuario.usuario ?? "Sin usuario",
                Nombre = usuario.Nombre ?? "Sin nombre",
                Correo = usuario.Correo ?? "Sin correo",
                Telefono = usuario.Telefono ?? "No registrado",
                Direccion = usuario.Direccion ?? "No especificada",
                TipoUsuario = usuario.Tipo_Usuario ?? "Turista",
                Idiomas = usuario.Idiomas ?? "No especificado",
                Nacionalidad = usuario.Nacionalidad ?? "No especificada",
                Especialidad = usuario.Especialidad ?? "No especificada",
                Experiencia = usuario.Experiencia ?? "No especificada",
                Disponibilidad = usuario.Disponibilidad ?? "No especificada",
                TarifaHora = usuario.TarifaHora ?? 0m,
                TarifaTour = usuario.TarifaTour ?? 0m,
                foto_perfil = string.IsNullOrEmpty(usuario.foto_perfil) ? "~/img/fotoperfil.png" : usuario.foto_perfil
            };

            return View("~/Views/Perfil/modificar-perfil.cshtml", perfilViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPerfil(Perfil model, IFormFile FotoPerfil)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("IniciarSesion", "Login");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return RedirectToAction("IniciarSesion", "Login");

            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null)
                return RedirectToAction("IniciarSesion", "Login");

            // Modificar los detalles del usuario
            usuario.usuario = model.usuario ?? usuario.usuario;
            usuario.Nombre = model.Nombre ?? usuario.Nombre;
            usuario.Correo = model.Correo ?? usuario.Correo;
            usuario.Telefono = model.Telefono ?? usuario.Telefono;
            usuario.Direccion = model.Direccion ?? usuario.Direccion;
            usuario.Idiomas = model.Idiomas ?? usuario.Idiomas;
            usuario.Especialidad = model.Especialidad ?? usuario.Especialidad;

            // Si es guía, actualizar los campos específicos
            if (usuario.Tipo_Usuario == "Guía")
            {
                usuario.Nacionalidad = model.Nacionalidad ?? usuario.Nacionalidad;
                usuario.Experiencia = model.Experiencia ?? usuario.Experiencia;
                usuario.Disponibilidad = model.Disponibilidad ?? usuario.Disponibilidad;
                usuario.TarifaHora = (decimal?)model.TarifaHora ?? 0;
                usuario.TarifaTour = (decimal?)model.TarifaTour ?? 0;
            }

            // Guardar la foto de perfil si se sube una nueva
            if (FotoPerfil != null && FotoPerfil.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                var uniqueFileName = $"{Guid.NewGuid()}_{FotoPerfil.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await FotoPerfil.CopyToAsync(fileStream);
                }

                usuario.foto_perfil = $"/img/{uniqueFileName}";
            }

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            // Mensaje de éxito
            TempData["Mensaje"] = "Tu perfil ha sido actualizado correctamente.";

            return RedirectToAction("Perfil");
        }


        [HttpPost]
        public async Task<IActionResult> ValidarYActualizarContrasena([FromBody] CambiarContrasenaDTO datos)
        {
            if (!User.Identity.IsAuthenticated)
                return Unauthorized();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return BadRequest(new { success = false, mensaje = "Usuario no válido" });

            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null)
                return NotFound(new { success = false, mensaje = "Usuario no encontrado" });

            var contrasenaActualHash = EncryptPassword(datos.actual);

            if (usuario.Contrasena != contrasenaActualHash)
            {
                return BadRequest(new { success = false, mensaje = "La contraseña actual es incorrecta" });
            }

            usuario.Contrasena = EncryptPassword(datos.nueva);
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        private string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return null;

            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }


    }
}