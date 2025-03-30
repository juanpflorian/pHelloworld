using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pHelloworld.Models
{
    public class Reserva
    {
        [Key]
        [Column("id_reserva")]
        public int IdReserva { get; set; }

        [ForeignKey("Turista")]
        [Column("id_turista")]
        public int IdTurista { get; set; }

        public Usuario Turista { get; set; }

        [ForeignKey("Guia")]
        [Column("id_guia")]
        public int IdGuia { get; set; }

        [ForeignKey("Plan")]
        [Column("id_plan")]
        public int IdPlan { get; set; }

        public Plan Plan { get; set; }

        public Usuario? Guia { get; set; }

        [Column("fecha_reserva")]
        public DateTime FechaReserva { get; set; } = DateTime.UtcNow;

        [Column("fecha_programada")]
        public DateTime FechaProgramada { get; set; }

        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [Column("duracion")]
        public string? Duracion { get; set; }

        [Column("precio", TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        [Required]
        [Column("estado")]
        public string? Estado { get; set; } = "Pendiente";
    }
}
