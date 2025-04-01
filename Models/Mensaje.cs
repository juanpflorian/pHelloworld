using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pHelloworld.Models
{
    public class Mensaje
    {
        [Key]
        public int IdMensaje { get; set; }

        [ForeignKey("Emisor")]
        public int IdEmisor { get; set; }
        public Usuario Emisor { get; set; }

        [ForeignKey("Receptor")]
        public int IdReceptor { get; set; }
        public Usuario Receptor { get; set; }

        [MaxLength(1000)]
        public string? Contenido { get; set; }

        public string? ImagenRuta { get; set; }
        public DateTime FechaEnvio { get; set; } = DateTime.UtcNow;

        public bool Leido { get; set; } = false;
    }
}
