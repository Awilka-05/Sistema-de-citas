using Microsoft.EntityFrameworkCore;
using SistemaGestionCitas.Infrastructure.Persistence.BdContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<SistemaCitasDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add custom services
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
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