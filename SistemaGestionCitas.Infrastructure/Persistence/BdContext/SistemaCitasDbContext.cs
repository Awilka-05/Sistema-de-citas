using Microsoft.EntityFrameworkCore;
using SistemaGestionCitas.Domain.Entities;


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
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relaciones Cita
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

            modelBuilder.Entity<Servicio>()
                .Property(s => s.Precio)
                .HasPrecision(18, 2);

            // Conversiones enum a string
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Rol)
                .HasConversion<string>();

            modelBuilder.Entity<Cita>()
                .Property(c => c.Estado)
                .HasConversion<string>();

            // RowVersion
            modelBuilder.Entity<Cita>()
                .Property(c => c.RowVersion)
                .IsRowVersion();

            // Llaves primarias
            modelBuilder.Entity<Usuario>().HasKey(u => u.IdUsuario);
            modelBuilder.Entity<Servicio>().HasKey(s => s.ServicioId);
            modelBuilder.Entity<Horario>().HasKey(h => h.HorarioId);
            modelBuilder.Entity<Lugar>().HasKey(l => l.LugarId);
            modelBuilder.Entity<ConfiguracionTurno>().HasKey(ct => ct.TurnoId);
            modelBuilder.Entity<Cita>().HasKey(c => c.IdCita);

            // Mapear Value Objects como Owned Types
            modelBuilder.Entity<Usuario>().OwnsOne(u => u.Nombre, n =>
            {
                n.Property(x => x.Value).HasColumnName("Nombre");
            });

            modelBuilder.Entity<Usuario>().OwnsOne(u => u.Cedula, c =>
            {
                c.Property(x => x.Value).HasColumnName("Cedula");
            });

            modelBuilder.Entity<Usuario>().OwnsOne(u => u.Correo, e =>
            {
                e.Property(x => x.Value).HasColumnName("Correo");
            });
        }
    }
}