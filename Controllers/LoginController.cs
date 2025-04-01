using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using pHelloworld.Data;
using pHelloworld.DTOs;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace pHelloworld.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult IniciarSesion()
        {
            return View("~/Views/usuario/IniciarSesion.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(LoginDTO model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/usuario/IniciarSesion.cshtml", model);

            var usuario = await _context.GetCredencial(model.Correo);
            var contrasenaEncriptada = EncryptPassword(model.Contrasena);

            // Validación de existencia y coincidencia de contraseña
            if (usuario == null || usuario.Contrasena == null ||
                !usuario.Contrasena.Equals(contrasenaEncriptada, StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View("~/Views/usuario/IniciarSesion.cshtml", model);
            }

            // Construcción de claims (datos del usuario para autenticación)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.id_usuario.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre ?? "Sin Nombre"),
                new Claim(ClaimTypes.Email, usuario.Correo ?? "Sin Correo"),
                new Claim(ClaimTypes.Role, usuario.Tipo_Usuario ?? "Turista"),
                new Claim("Telefono", usuario.Telefono ?? "No Disponible"),
                new Claim("Direccion", usuario.Direccion ?? "No Disponible"),
                new Claim("Idiomas", usuario.Idiomas ?? "No Especificado"),
                new Claim("Especialidad", usuario.Especialidad ?? "No Especificado"),
                new Claim("Experiencia", usuario.Experiencia ?? "No Especificado"),
                new Claim("Disponibilidad", usuario.Disponibilidad ?? "No Especificado"),
                new Claim("TarifaHora", usuario.TarifaHora?.ToString() ?? "0"),
                new Claim("TarifaTour", usuario.TarifaTour?.ToString() ?? "0")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);

            return RedirectToAction("Perfil", "Perfil");
        }

        // Encripta la contraseña usando SHA-256
        private string EncryptPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        [HttpGet]
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
