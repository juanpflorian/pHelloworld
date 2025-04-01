using System.ComponentModel.DataAnnotations;

namespace pHelloworld.Models
{
    public class Perfil
    {
        [Key]
        public int id_usuario { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(15, ErrorMessage = "Máximo 15 caracteres")]
        public string? usuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [RegularExpression(@"^\d+$", ErrorMessage = "El teléfono debe contener solo números")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; }

        public string TipoUsuario { get; set; }

        [Required(ErrorMessage = "El campo Idiomas es obligatorio")]
        public string Idiomas { get; set; }

        public string Nacionalidad { get; set; }
        public string Especialidad { get; set; }
        public string Experiencia { get; set; }
        public string Disponibilidad { get; set; }

        
        public decimal TarifaHora { get; set; }

        
        public decimal TarifaTour { get; set; }

        public string foto_perfil { get; set; }
    }
}
