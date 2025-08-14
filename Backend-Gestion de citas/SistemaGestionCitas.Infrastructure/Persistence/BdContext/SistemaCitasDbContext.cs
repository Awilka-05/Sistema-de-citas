using Microsoft.EntityFrameworkCore;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Enums;

namespace SistemaGestionCitas.Infrastructure.Persistence.BdContext
{
    public class SistemaCitasDbContext : DbContext
    {

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Lugar> Lugares { get; set; }
        public DbSet<ConfiguracionTurno> ConfiguracionesTurnos { get; set; }
        public DbSet<Cita> Citas { get; set; }

        public SistemaCitasDbContext(DbContextOptions<SistemaCitasDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Citas)
                .HasForeignKey(c => c.IdUsuario);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.ConfiguracionTurno)
                .WithMany(t => t.Citas)
                .HasForeignKey(c => c.TurnoId);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Lugar)
                .WithMany(l => l.Citas)
                .HasForeignKey(c => c.LugarId);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Servicio)
                .WithMany(s => s.Citas)
                .HasForeignKey(c => c.ServicioId);

            modelBuilder.Entity<ConfiguracionTurno>()
                .HasOne(ct => ct.Horario)
                .WithMany(h => h.ConfiguracionesTurnos)
                .HasForeignKey(ct => ct.HorariosId);

            modelBuilder.Entity<Usuario>()
            .Property(u => u.Rol)
            .HasConversion<string>();

            modelBuilder.Entity<Cita>()
           .Property(c => c.Estado)
           .HasConversion<string>();

            // Configurar RowVersion para concurrencia
            modelBuilder.Entity<Cita>()
                .Property(c => c.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<Usuario>().HasKey(u => u.IdUsuario);
            modelBuilder.Entity<Servicio>().HasKey(s => s.ServicioId);
            modelBuilder.Entity<Horario>().HasKey(h => h.HorarioId);
            modelBuilder.Entity<Lugar>().HasKey(l => l.LugarId);
            modelBuilder.Entity<ConfiguracionTurno>().HasKey(ct => ct.TurnoId);
            modelBuilder.Entity<Cita>().HasKey(c => c.IdCita);

            modelBuilder.Entity<Usuario>().HasData(
            new Usuario
            {
                IdUsuario = 1,
                Nombre = "Administrador",
                Cedula = "12345678910",
                Correo = "admin@dominio.com",
                Contrasena = "123456",
                Rol = RolUsuario.Admin,
                FechaNacimiento = new DateTime(2000, 1, 1),
                Activo = true,
               
            }
            );
        }
    }
}



