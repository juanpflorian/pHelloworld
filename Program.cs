using pHelloworld.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La cadena de conexión 'DefaultConnection' no está configurada en appsettings.json.");
}

Console.WriteLine($"Connection String: {connectionString}"); // Para verificar la cadena de conexión

// Configurar Entity Framework con MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// Agregar servicios MVC
builder.Services.AddControllersWithViews();

// Habilitar sesiones
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

// Registrar IHttpContextAccessor ?? Agrega esta línea
builder.Services.AddHttpContextAccessor();

// Configurar autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/IniciarSesion";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);

        // Configurar el evento de redirección al login
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    });

// Configurar autorización
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EsGuia", policy =>
        policy.RequireClaim("TipoUsuario", "Guía"));
});

// Construir la aplicación después de configurar los servicios
var app = builder.Build();

// Configurar el middleware de la aplicación
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession(); // Habilitar sesiones antes de la autenticación

app.UseAuthentication(); // Importante: primero la autenticación
app.UseAuthorization();  // Luego la autorización

// Configurar rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "guias",
    pattern: "Guias",
    defaults: new { controller = "Guias", action = "guias" }
);

// Ejecutar la aplicación
app.Run();
