using Microsoft.AspNetCore.Hosting;
using SIRS.Application.Interfaces;
using SIRS.Application.Services;
using SIRS.Data.Repository;
using SIRS.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

builder.Services.AddScoped<IEdificioAppService, EdificioAppService>(); 
builder.Services.AddScoped<IEdificioRepository, EdificioRepository>();
builder.Services.AddAutoMapper(typeof(Program));


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
