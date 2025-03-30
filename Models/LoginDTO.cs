using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace pHelloworld.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo no válido.")]
        [BindProperty]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [BindProperty]
        public string Contrasena { get; set; }
    }
}
 