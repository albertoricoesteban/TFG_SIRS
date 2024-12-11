using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SIRS.Application.Interfaces;
using SIRS.Application.Services;
using SIRS.Data.Context;
using SIRS.Data.Repository;
using SIRS.Domain.Interfaces;
using Newtonsoft.Json;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configura la autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
            ClockSkew = TimeSpan.Zero // Opcional para evitar problemas de sincronización de tiempo
        };
    });



// Agregar servicios de la aplicación
builder.Services.AddScoped<SIRS.Service.API.AuthService>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

// Configuración de Swagger (opcional, si quieres documentación de la API)
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// Configuración de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SIRSDBConnection")));

// Registro de los servicios necesarios
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IEdificioRepository, EdificioRepository>();
builder.Services.AddScoped<IEstadoSalaRepository, EstadoSalaRepository>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<ISalaRepository, SalaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IEdificioAppService, EdificioAppService>();
builder.Services.AddScoped<IUsuarioAppService, UsuarioAppService>();
builder.Services.AddScoped<IEstadoSalaAppService, EstadoSalaAppService>();
builder.Services.AddScoped<IReservaAppService, ReservaAppService>();
builder.Services.AddScoped<ISalaAppService, SalaAppService>();
builder.Services.AddScoped<IRolAppService, RolAppService>();

var app = builder.Build();

// Configuración del pipeline de solicitudes HTTP
app.UseHttpsRedirection();

// Aquí es donde se deben agregar los middlewares de autenticación y autorización
app.UseAuthentication(); // Necesario para autenticar al usuario
app.UseAuthorization();  // Necesario para autorizar las rutas protegidas

app.MapControllers(); // Mapea los controladores de la API

// Si deseas habilitar Swagger para pruebas o documentación
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.Run();
