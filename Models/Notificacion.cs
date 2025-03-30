using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pHelloworld.Models
{
    public class Notificacion
    {
        [Key]
        public int IdNotificacion { get; set; }

        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public string Mensaje { get; set; }

        public bool Leido { get; set; } = false;

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // 🔗 Asociación opcional con Plan, Reserva o Mensaje
        public int? IdPlan { get; set; }
        public Plan? Plan { get; set; }

        public int? IdReserva { get; set; }
        public Reserva? Reserva { get; set; }

        public int? IdMensaje { get; set; }
        public Mensaje? MensajeRelacionado { get; set; }
    }
}
