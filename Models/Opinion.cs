using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation; // ⬅️ importante

public class Opinion
{
    [Key]
    public int IdOpinion { get; set; }

    [Required]
    public int IdTurista { get; set; }

    [ValidateNever] // ⬅️ Ignorar validación
    [ForeignKey("IdTurista")]
    public Usuario Turista { get; set; }

    [Required]
    public int IdGuia { get; set; }

    [ValidateNever] // ⬅️ Ignorar validación
    [ForeignKey("IdGuia")]
    public Usuario Guia { get; set; }

    [Range(1, 5)]
    public int Calificacion { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Comentario { get; set; }

    public DateTime Fecha { get; set; } = DateTime.UtcNow;
}
