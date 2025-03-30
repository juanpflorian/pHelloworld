using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pHelloworld.Data;
using pHelloworld.Models;

namespace pHelloworld.Controllers
{
    public class RegistroController : Controller
    {
        private readonly AppDbContext _context;

        public RegistroController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Registrarse()
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                TempData["Mensaje"] = $"Ya hay una sesión activa: {HttpContext.Session.GetString("Usuario")}";
                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/usuario/Registrarse.cshtml", new RegistroViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registrarse(RegistroViewModel model)
        {
            if (HttpContext.Session.GetString("Usuario") != null)
            {
                TempData["Mensaje"] = $"Ya hay una sesión activa: {HttpContext.Session.GetString("Usuario")}";
                return RedirectToAction("Index", "Home");
            }

            if (string.IsNullOrEmpty(model.Contrasena) || model.Contrasena.Length < 8)
            {
                ModelState.AddModelError("Contrasena", "La contraseña debe tener al menos 8 caracteres");
                return View("~/Views/Usuario/Registrarse.cshtml", model);
            }

            if (_context.Usuarios.Any(u => u.Correo == model.Correo))
            {
                ModelState.AddModelError("Correo", "El correo ya está registrado");
                return View("~/Views/usuario/Registrarse.cshtml", model);
            }

            var usuario = new Usuario
            {
                usuario = string.IsNullOrEmpty(model.usuario) ? "Sin diligenciar" : model.usuario,
                Nombre = string.IsNullOrEmpty(model.Nombre) ? "Sin diligenciar" : model.Nombre,
                Correo = model.Correo ?? string.Empty,
                Contrasena = EncryptPassword(model.Contrasena),
                Fecha_Creacion = DateTime.Now,
                Tipo_Usuario = "Turista",

                
                Telefono = "Sin diligenciar",
                Direccion = "Sin diligenciar",
                Nacionalidad = "Sin diligenciar",
                foto_perfil = "/img/fotoperfil.png",
                Idiomas = "Sin diligenciar",
                Especialidad = "Sin diligenciar",
                Experiencia = "Sin diligenciar",
                Disponibilidad = "Sin diligenciar",
                TarifaHora = 0m,
                TarifaTour = 0m
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            TempData["Mensaje"] = "Usuario registrado exitosamente";
            Console.WriteLine($"Nombre: {usuario.Nombre}, Correo: {usuario.Correo}, Contraseña Encriptada: {usuario.Contrasena}");

            return RedirectToAction("IniciarSesion", "Login");
        }

        private string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Error: La contraseña es nula o vacía.");
                return null;
            }

            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
