using System.ComponentModel.DataAnnotations;

namespace pHelloworld.Models
{
    public class Perfil
    {

        [Key]
        public int id_usuario { get; set; }

        [StringLength(15)]
        public string? usuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string TipoUsuario { get; set; }
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
