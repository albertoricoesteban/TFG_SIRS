using Microsoft.EntityFrameworkCore;
using SIRS.Application.Interfaces;
using SIRS.Application.Services;
using SIRS.Data.Context;
using SIRS.Data.Repository;
using SIRS.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

// Registrar ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SIRSDBConnection")));

// Registrar servicios
builder.Services.AddScoped<IEdificioAppService, EdificioAppService>();
builder.Services.AddScoped<IEdificioRepository, EdificioRepository>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
