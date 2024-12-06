using Microsoft.EntityFrameworkCore;
using SIRS.Application.Interfaces;
using SIRS.Application.Services;
using SIRS.Data.Context;
using SIRS.Data.Repository;
using SIRS.Domain.Interfaces;
using SIRS.ApliClient; // Asegúrate de agregar esta línea para usar ApiClientService

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

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Registrar ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SIRSDBConnection")));


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

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Esto asegura que los archivos estáticos se sirvan correctamente
app.UseSession();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();

app.Run();
