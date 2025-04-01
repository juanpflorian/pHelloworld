using System.ComponentModel.DataAnnotations;

namespace pHelloworld.Models
{
    public class RegistroViewModel
    {
        [Required, StringLength(15)]
        public string usuario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una nacionalidad")]
        public string Nacionalidad { get; set; }


        [Required, StringLength(100)]
        public string Nombre { get; set; }  

        [Required, EmailAddress, StringLength(100)]
        public string Correo { get; set; }  

        [Required, DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string Contrasena { get; set; }  
        
        [Required, DataType(DataType.Password)]
        [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContrasena { get; set; }  

        public DateTime FechaCreacion { get; set; } = DateTime.Now;  

        public string TipoUsuario { get; set; } = "Turista";  
    }
}
