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

// Configura la autenticaci�n JWT
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
            ClockSkew = TimeSpan.Zero // Opcional para evitar problemas de sincronizaci�n de tiempo
        };
    });



// Agregar servicios de la aplicaci�n
builder.Services.AddScoped<SIRS.Service.API.AuthService>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

// Configuraci�n de Swagger (opcional, si quieres documentaci�n de la API)
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// Configuraci�n de la base de datos
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

// Configuraci�n del pipeline de solicitudes HTTP
app.UseHttpsRedirection();

// Aqu� es donde se deben agregar los middlewares de autenticaci�n y autorizaci�n
app.UseAuthentication(); // Necesario para autenticar al usuario
app.UseAuthorization();  // Necesario para autorizar las rutas protegidas

app.MapControllers(); // Mapea los controladores de la API

// Si deseas habilitar Swagger para pruebas o documentaci�n
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.Run();
