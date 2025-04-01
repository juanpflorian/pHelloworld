using pHelloworld.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Usuario
{
    [Key]  
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int id_usuario { get; set; }

    public string usuario { get; set; }
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Telefono { get; set; }
    public string Direccion { get; set; }
    public string Tipo_Usuario { get; set; }
    public string Contrasena { get; set; }
    public DateTime Fecha_Creacion { get; set; }
    public string foto_perfil { get; set; }
    public string Idiomas { get; set; }
    public string Nacionalidad { get; set; }
    public string Especialidad { get; set; }
    public string Experiencia { get; set; }
    public string Disponibilidad { get; set; }
    public decimal? TarifaHora { get; set; }
    public decimal? TarifaTour { get; set; }
    public ICollection<Opinion> OpinionesRecibidas { get; set; }

}
