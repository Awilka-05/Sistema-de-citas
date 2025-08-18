using Microsoft.EntityFrameworkCore;
using SistemaGestionCitas.Domain.Entities;
using SistemaGestionCitas.Domain.Value_Objects;


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
           // Mapeo de Usuario
            modelBuilder.Entity<Usuario>().ToTable("usuarios");
            modelBuilder.Entity<Usuario>().HasKey(u => u.IdUsuario);
            modelBuilder.Entity<Usuario>()
                .Property(u => u.IdUsuario).HasColumnName("id_usuario");
            modelBuilder.Entity<Usuario>()
                .Property(u => u.FechaNacimiento).HasColumnName("fecha_nacimiento");
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Nombre).HasColumnName("nombre");
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Cedula).HasColumnName("cedula");
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Correo).HasColumnName("correo");
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Telefono).HasColumnName("telefono");
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Contrasena).HasColumnName("contrasena");
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Rol).HasColumnName("rol");
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Activo).HasColumnName("activo");
            // Conversiones de Value Objects
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Nombre)
                .HasConversion(v => v.Value, v => new Nombre(v));
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Cedula)
                .HasConversion(v => v.Value, v => new Cedula(v));
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Correo)
                .HasConversion(v => v.Value, v => new Correo(v));

            // Mapeo de Servicio
            modelBuilder.Entity<Servicio>().ToTable("servicios");
            modelBuilder.Entity<Servicio>().HasKey(s => s.ServicioId);
            modelBuilder.Entity<Servicio>()
                .Property(s => s.ServicioId).HasColumnName("id");
            modelBuilder.Entity<Servicio>()
                .Property(s => s.Nombre).HasColumnName("nombre");
            modelBuilder.Entity<Servicio>()
                .Property(s => s.Precio).HasColumnName("precio").HasPrecision(10, 2);

            // Mapeo de Horario
            modelBuilder.Entity<Horario>().ToTable("horarios");
            modelBuilder.Entity<Horario>().HasKey(h => h.HorarioId);
            modelBuilder.Entity<Horario>()
                .Property(h => h.HorarioId).HasColumnName("id");
            modelBuilder.Entity<Horario>()
                .Property(h => h.HoraInicio).HasColumnName("hora_inicio");
            modelBuilder.Entity<Horario>()
                .Property(h => h.HoraFin).HasColumnName("hora_fin");
            modelBuilder.Entity<Horario>()
                .Property(h => h.Descripcion).HasColumnName("descripcion");

            // Mapeo de Lugar
            modelBuilder.Entity<Lugar>().ToTable("lugar");
            modelBuilder.Entity<Lugar>().HasKey(l => l.LugarId);
            modelBuilder.Entity<Lugar>()
                .Property(l => l.LugarId).HasColumnName("id");
            modelBuilder.Entity<Lugar>()
                .Property(l => l.Nombre).HasColumnName("nombre");

            // Mapeo de ConfiguracionTurno
            modelBuilder.Entity<ConfiguracionTurno>().ToTable("configuracion_turnos");
            modelBuilder.Entity<ConfiguracionTurno>().HasKey(ct => ct.TurnoId);
            modelBuilder.Entity<ConfiguracionTurno>()
                .Property(ct => ct.TurnoId).HasColumnName("id");
            modelBuilder.Entity<ConfiguracionTurno>()
                .Property(ct => ct.FechaInicio).HasColumnName("fecha_inicio");
            modelBuilder.Entity<ConfiguracionTurno>()
                .Property(ct => ct.FechaFin).HasColumnName("fecha_fin");
            modelBuilder.Entity<ConfiguracionTurno>()
                .Property(ct => ct.HorariosId).HasColumnName("horarios_id");
            modelBuilder.Entity<ConfiguracionTurno>()
                .Property(ct => ct.CantidadEstaciones).HasColumnName("cantidad_estaciones");
            modelBuilder.Entity<ConfiguracionTurno>()
                .Property(ct => ct.DuracionMinutos).HasColumnName("duracion_minutos");
            modelBuilder.Entity<ConfiguracionTurno>()
                .Property(ct => ct.AunAceptaCitas).HasColumnName("aun_acepta_citas");

            // Mapeo de Cita
            modelBuilder.Entity<Cita>().ToTable("cita");
            modelBuilder.Entity<Cita>().HasKey(c => c.IdCita);
            modelBuilder.Entity<Cita>()
                .Property(c => c.IdCita).HasColumnName("id_cita");
            modelBuilder.Entity<Cita>()
                .Property(c => c.IdUsuario).HasColumnName("id_usuario");
            modelBuilder.Entity<Cita>()
                .Property(c => c.TurnoId).HasColumnName("turno_id");
            modelBuilder.Entity<Cita>()
                .Property(c => c.LugarId).HasColumnName("lugar_id");
            modelBuilder.Entity<Cita>()
                .Property(c => c.ServicioId).HasColumnName("servicio_id");
            modelBuilder.Entity<Cita>()
                .Property(c => c.Estado).HasColumnName("estado");
            modelBuilder.Entity<Cita>()
                .Property(c => c.RowVersion).HasColumnName("row_version");
            modelBuilder.Entity<Cita>()
                .Property(c => c.FechaCita).HasColumnName("fecha_cita");
        

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
        }
    }
}