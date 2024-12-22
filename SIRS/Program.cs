using Microsoft.EntityFrameworkCore;
using SIRS.Application.Interfaces;
using SIRS.Application.Services;
using SIRS.Data.Context;
using SIRS.Data.Repository;
using SIRS.Domain.Interfaces;
using SIRS.ApliClient;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Habilitar la memoria de sesión (opcionalmente usar Redis o SQL Server)
builder.Services.AddDistributedMemoryCache();

// Configurar la sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duración de la sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Registrar la autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Ruta de inicio de sesión
        options.AccessDeniedPath = "/Account/UnAuthorize"; // Ruta de acceso denegado
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Registrar ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SIRSDBConnection")));

// Configuración de MVC y JSON
builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Registrar servicios
builder.Services.AddScoped<IEdificioAppService, EdificioAppService>();
builder.Services.AddScoped<IEdificioRepository, EdificioRepository>();

builder.Services.AddScoped<ISalaAppService, SalaAppService>();
builder.Services.AddScoped<ISalaRepository, SalaRepository>();

builder.Services.AddScoped<IEstadoSalaAppService, EstadoSalaAppService>();
builder.Services.AddScoped<IEstadoSalaRepository, EstadoSalaRepository>();

builder.Services.AddScoped<IReservaAppService, ReservaAppService>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();

builder.Services.AddScoped<IUsuarioAppService, UsuarioAppService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddAutoMapper(typeof(Program));

// Registrar ApiClientService
builder.Services.AddHttpClient<ApiClientService>();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
app.UseHttpsRedirection();
app.UseStaticFiles(); // Esto asegura que los archivos estáticos se sirvan correctamente

// Usar sesión y autenticación
app.UseSession(); // Necesario para manejar sesiones
app.UseAuthentication(); // Usar autenticación
app.UseAuthorization(); // Usar autorización
app.MapControllers();

// Definir rutas y controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Ejecutar la aplicación
app.Run();
