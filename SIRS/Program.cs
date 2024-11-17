using Microsoft.EntityFrameworkCore;
using SIRS.Application.Interfaces;
using SIRS.Application.Services;
using SIRS.Data.Context;
using SIRS.Data.Repository;
using SIRS.Domain.Interfaces;
using SIRS.ApliClient; // Aseg�rate de agregar esta l�nea para usar ApiClientService

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

// Registrar ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SIRSDBConnection")));

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

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();

app.Run();
