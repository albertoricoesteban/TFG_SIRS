using Microsoft.EntityFrameworkCore;
using SIRS.Application.Interfaces;
using SIRS.Application.Services;
using SIRS.Data.Context;
using SIRS.Data.Repository;
using SIRS.Domain.Interfaces;
using SIRS.ApliClient;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Habilitar la memoria de sesi�n (opcionalmente usar Redis o SQL Server)
builder.Services.AddDistributedMemoryCache();

// Configurar la sesi�n
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Duraci�n de la sesi�n
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

// Registrar la autenticaci�n con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Ruta de inicio de sesi�n
        options.AccessDeniedPath = "/Account/UnAuthorize"; // Ruta de acceso denegado
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Registrar ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SIRSDBConnection")));

// Configuraci�n de MVC y JSON
builder.Services.AddControllersWithViews()
    .AddDataAnnotationsLocalization()
    .AddJsonOptions(options => {
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
app.UseStaticFiles(); // Esto asegura que los archivos est�ticos se sirvan correctamente

// Usar sesi�n y autenticaci�n
app.UseSession(); // Necesario para manejar sesiones
app.UseAuthentication(); // Usar autenticaci�n
app.UseAuthorization(); // Usar autorizaci�n

// Definir rutas y controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();

// Ejecutar la aplicaci�n
app.Run();
