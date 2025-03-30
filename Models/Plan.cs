using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pHelloworld.Models
{
    public class Plan
    {
        [Key]
        [Column("id_plan")]
        public int IdPlan { get; set; }

        [Column("id_guia")]
        public int? IdGuia { get; set; }

        [Required]
        [Column("nombre_plan")]
        public string NombrePlan { get; set; }

        [Required]
        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Required]
        [Column("duracion")]
        public string Duracion { get; set; }

        [Required]
        [Column("numero_personas")]
        public string NumeroPersonas { get; set; }

        [Column("precio")]
        public decimal? Precio { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        public Usuario Guia { get; set; }
    }
}
