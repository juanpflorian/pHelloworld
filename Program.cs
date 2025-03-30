using pHelloworld.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Obtener la cadena de conexi�n desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("La cadena de conexi�n 'DefaultConnection' no est� configurada en appsettings.json.");
}

Console.WriteLine($"Connection String: {connectionString}"); // Para verificar la cadena de conexi�n

// Configurar Entity Framework con MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// Agregar servicios MVC
builder.Services.AddControllersWithViews();

// Habilitar sesiones
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

// Registrar IHttpContextAccessor ?? Agrega esta l�nea
builder.Services.AddHttpContextAccessor();

// Configurar autenticaci�n con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/IniciarSesion";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);

        // Configurar el evento de redirecci�n al login
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
    });

// Configurar autorizaci�n
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EsGuia", policy =>
        policy.RequireClaim("TipoUsuario", "Gu�a"));
});

// Construir la aplicaci�n despu�s de configurar los servicios
var app = builder.Build();

// Configurar el middleware de la aplicaci�n
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession(); // Habilitar sesiones antes de la autenticaci�n

app.UseAuthentication(); // Importante: primero la autenticaci�n
app.UseAuthorization();  // Luego la autorizaci�n

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

// Ejecutar la aplicaci�n
app.Run();
