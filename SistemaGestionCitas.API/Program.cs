using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SistemaGestionCitas.Application.Services;
using SistemaGestionCitas.Application.UseCases;
using SistemaGestionCitas.Application.Validators;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Interfaces.Repositories;
using SistemaGestionCitas.Domain.Interfaces.Services;
using SistemaGestionCitas.Infrastructure.JWT;
using SistemaGestionCitas.Infrastructure.Persistence.BdContext;
using SistemaGestionCitas.Infrastructure.Repositories;
using SistemaGestionCitas.Infrastructure.Repositories.Logger;
using SistemaGestionCitas.Infrastructure.Services.Correo.Strategy;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<SistemaCitasDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de Aplicación/Casos de Uso (Services)
builder.Services.AddScoped<IRegistrarUsuario, RegistrarUsuarioService>();
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

// JWT & Auth - CONFIGURACIÓN CORREGIDA
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<UserValidator>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

var jwtSecret = builder.Configuration["Jwt:Secret"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

// Verificar que los valores JWT no sean null
if (string.IsNullOrWhiteSpace(jwtSecret))
    throw new InvalidOperationException("JWT:Secret no está configurado en appsettings.json");

if (string.IsNullOrWhiteSpace(jwtIssuer))
    throw new InvalidOperationException("JWT:Issuer no está configurado en appsettings.json");

if (string.IsNullOrWhiteSpace(jwtAudience))
    throw new InvalidOperationException("JWT:Audience no está configurado en appsettings.json");

// Verificar que la clave tenga longitud mínima
if (jwtSecret.Length < 32)
    throw new InvalidOperationException("JWT:Secret debe tener al menos 32 caracteres");

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.MapInboundClaims = false; // Mantener los nombres originales de los claims
        
        var key = Encoding.UTF8.GetBytes(jwtSecret);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero // Opcional: eliminar tolerancia de tiempo
        };
    });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

CorreoSender.SetConfiguration(builder.Configuration);

// My SingletonLogger
builder.Logging.AddConsole(); // consola sigue activa
builder.Logging.AddProvider(new SingletonLoggerProvider()); // nuestro logger singleton a archivo

//Mapster 
builder.Services.AddSingleton(TypeAdapterConfig.GlobalSettings);
builder.Services.AddScoped<IMapper, ServiceMapper>();

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

app.UseCors(); // Si usas CORS
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();