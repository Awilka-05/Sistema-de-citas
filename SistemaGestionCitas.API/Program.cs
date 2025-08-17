using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using SistemaGestionCitas.Application.Services;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Interfaces.Services;
using SistemaGestionCitas.Infrastructure.Persistence.BdContext;
using SistemaGestionCitas.Infrastructure.Repositories;
using SistemaGestionCitas.Infrastructure.Services.Correo.Strategy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<SistemaCitasDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ILugarService, LugarService>();
builder.Services.AddScoped<IServicioService, ServicioService>();

builder.Services.AddScoped<IRepository<Lugar, short>, LugarRepository>();
builder.Services.AddScoped<IRepository<Servicio, short>, ServicioRepository>();


// Add custom services
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

CorreoSender.SetConfiguration(builder.Configuration);

//Mapster 

builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddScoped<IMapper, ServiceMapper>();

builder.Logging.AddProvider(new SistemaGestionCitas.Application.Services.LoggerProvider(
    builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty));
// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();