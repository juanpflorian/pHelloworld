using Microsoft.EntityFrameworkCore;
using pHelloworld.Models;
using System.Threading.Tasks;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<Plan> Planes { get; set; }
    public DbSet<Mensaje> Mensaje { get; set; }
    public DbSet<Notificacion> Notificacion { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relaciones existentes
        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.Turista)
            .WithMany()
            .HasForeignKey(r => r.IdTurista)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reserva>()
            .HasOne(r => r.Guia)
            .WithMany()
            .HasForeignKey(r => r.IdGuia)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Plan>()
            .HasOne(p => p.Guia)
            .WithMany()
            .HasForeignKey(p => p.IdGuia)
            .OnDelete(DeleteBehavior.Restrict);

        // 🆕 Relaciones para Mensaje
        modelBuilder.Entity<Mensaje>()
            .HasOne(m => m.Emisor)
            .WithMany()
            .HasForeignKey(m => m.IdEmisor)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Mensaje>()
            .HasOne(m => m.Receptor)
            .WithMany()
            .HasForeignKey(m => m.IdReceptor)
            .OnDelete(DeleteBehavior.Restrict);

        // 🆕 Relaciones para Notificación
        modelBuilder.Entity<Notificacion>()
            .HasOne(n => n.Usuario)
            .WithMany()
            .HasForeignKey(n => n.IdUsuario)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Notificacion>()
            .HasOne(n => n.Plan)
            .WithMany()
            .HasForeignKey(n => n.IdPlan)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Notificacion>()
            .HasOne(n => n.Reserva)
            .WithMany()
            .HasForeignKey(n => n.IdReserva)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Notificacion>()
            .HasOne(n => n.MensajeRelacionado)
            .WithMany()
            .HasForeignKey(n => n.IdMensaje)
            .OnDelete(DeleteBehavior.Restrict);

        // Clave primaria explícita (ya estaba)
        modelBuilder.Entity<Usuario>()
            .HasKey(u => u.id_usuario);

        base.OnModelCreating(modelBuilder);
    }


    public async Task<Usuario?> GetCredencial(string correo)
    {
        return await Usuarios
            .Where(u => u.Correo != null && u.Correo == correo) 
            .Select(u => new Usuario
            {
                id_usuario = u.id_usuario,
                usuario = u.usuario ?? string.Empty,
                Nombre = u.Nombre ?? string.Empty,
                Correo = u.Correo ?? string.Empty,
                Telefono = u.Telefono ?? string.Empty,
                Direccion = u.Direccion ?? string.Empty,
                Tipo_Usuario = u.Tipo_Usuario ?? "Turista",
                Contrasena = u.Contrasena ?? string.Empty,
                Fecha_Creacion = u.Fecha_Creacion,
                foto_perfil = u.foto_perfil ?? string.Empty,
                Idiomas = u.Idiomas ?? string.Empty,
                Nacionalidad = u.Nacionalidad ?? string.Empty,
                Especialidad = u.Especialidad ?? string.Empty,
                Experiencia = u.Experiencia ?? string.Empty,
                Disponibilidad = u.Disponibilidad ?? string.Empty,
                TarifaHora = u.TarifaHora ?? 0m,  
                TarifaTour = u.TarifaTour ?? 0m    
            })
            .FirstOrDefaultAsync();
    }


}