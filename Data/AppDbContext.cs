using Microsoft.EntityFrameworkCore;
using pHelloworld.Models;
using System.Threading.Tasks;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<Plan> Planes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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