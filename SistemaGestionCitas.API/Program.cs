using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SistemaGestionCitas.Application.Services;
using SistemaGestionCitas.Application.UseCases;
using SistemaGestionCitas.Application.Validators;
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

// Registro de Aplicación/Casos de Uso (Services)
builder.Services.AddScoped<ILugarService, LugarService>();
builder.Services.AddScoped<IServicioService, ServicioService>();
builder.Services.AddScoped<ICitaService, CitaService>();
builder.Services.AddScoped<IReservarCitaService, ReservarCita>();
builder.Services.AddScoped<ICancelarCitaService, CancelarCita>();
builder.Services.AddScoped<IConfiguracionTurnoService, ConfiguracionTurnoService>();
builder.Services.AddScoped<IHorarioService, HorarioService>();

// Registro de Validadores
builder.Services.AddScoped<ICitaValidator, CitaValidator>();


// Registra los repositorios concretos 
builder.Services.AddScoped<ICitaRepository, CitaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ILugarRepository, LugarRepository>();
builder.Services.AddScoped<IServicioRepository, ServicioRepository>();
builder.Services.AddScoped<IHorarioRepository, HorarioRepository>();
builder.Services.AddScoped<IConfiguracionTurnoRepository, ConfiguracionTurnoRepository>();


//Registros adicionales si usas un repositorio genérico
builder.Services.AddScoped<IRepository<Lugar, short>, LugarRepository>();
builder.Services.AddScoped<IRepository<Servicio, short>, ServicioRepository>();
builder.Services.AddScoped<IRepository<Horario, short>, HorarioRepository>();
builder.Services.AddScoped<IRepository<ConfiguracionTurno, int>, ConfiguracionTurnoRepository>();
builder.Services.AddScoped<IRepository<Usuario, int>, UsuarioRepository>();


builder.Services.AddControllers().AddJsonOptions(options =>
 {
     options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
 });


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