using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        public DbSet<FranjaHorario> FranjasHorarios { get; set; }

        public SistemaCitasDbContext(DbContextOptions<SistemaCitasDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Usuario
            modelBuilder.Entity<Usuario>().ToTable("usuarios");
            modelBuilder.Entity<Usuario>().HasKey(u => u.IdUsuario);
            modelBuilder.Entity<Usuario>().Property(u => u.IdUsuario).HasColumnName("id_usuario");
            modelBuilder.Entity<Usuario>().Property(u => u.FechaNacimiento).HasColumnName("fecha_nacimiento");
            modelBuilder.Entity<Usuario>().Property(u => u.Nombre).HasColumnName("nombre");
            modelBuilder.Entity<Usuario>().Property(u => u.Cedula).HasColumnName("cedula");
            modelBuilder.Entity<Usuario>().Property(u => u.Correo).HasColumnName("correo");
            modelBuilder.Entity<Usuario>().Property(u => u.Telefono).HasColumnName("telefono");
            modelBuilder.Entity<Usuario>().Property(u => u.Contrasena).HasColumnName("contrasena");
            modelBuilder.Entity<Usuario>().Property(u => u.Rol).HasColumnName("rol");
            modelBuilder.Entity<Usuario>().Property(u => u.Activo).HasColumnName("activo");

            // Conversiones de Value Objects
            
        var cedulaConverter = new ValueConverter<Cedula, string>(
            v => v.Value,            // Convierte de Cedula a string (para guardar en la DB)
            v => Cedula.Create(v).Value // Convierte de string a Cedula (al leer de la DB)
        );

            // Aplica el conversor a la propiedad Cedula de la entidad Usuario
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Cedula)
                .HasConversion(cedulaConverter);

            // Repite el proceso para Nombre y Correo si también los tienes mapeados en la DB
            var nombreConverter = new ValueConverter<Nombre, string>(
                v => v.Value,
                v => Nombre.Create(v).Value
            );
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Nombre)
                .HasConversion(nombreConverter);

            var correoConverter = new ValueConverter<Correo, string>(
                v => v.Value,
                v => Correo.Create(v).Value
            );
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Correo)
                .HasConversion(correoConverter);
            modelBuilder.Entity<Usuario>().Property(u => u.Nombre).HasConversion(v => v.Value, v => new Nombre(v));
            modelBuilder.Entity<Usuario>().Property(u => u.Cedula).HasConversion(v => v.Value, v => new Cedula(v));
            modelBuilder.Entity<Usuario>().Property(u => u.Correo).HasConversion(v => v.Value, v => new Correo(v));

            // Conversión de Enum a String
            modelBuilder.Entity<Usuario>().Property(u => u.Rol).HasConversion<string>();

            // Servicio
            modelBuilder.Entity<Servicio>().ToTable("servicios");
            modelBuilder.Entity<Servicio>().HasKey(s => s.ServicioId);
            modelBuilder.Entity<Servicio>().Property(s => s.ServicioId).HasColumnName("id");
            modelBuilder.Entity<Servicio>().Property(s => s.Nombre).HasColumnName("nombre");
            modelBuilder.Entity<Servicio>().Property(s => s.Precio).HasColumnName("precio").HasPrecision(18, 2);

            // Horario
            modelBuilder.Entity<Horario>().ToTable("horarios");
            modelBuilder.Entity<Horario>().HasKey(h => h.HorarioId);
            modelBuilder.Entity<Horario>().Property(h => h.HorarioId).HasColumnName("id");
            modelBuilder.Entity<Horario>().Property(h => h.HoraInicio).HasColumnName("hora_inicio");
            modelBuilder.Entity<Horario>().Property(h => h.HoraFin).HasColumnName("hora_fin");
            modelBuilder.Entity<Horario>().Property(h => h.Descripcion).HasColumnName("descripcion");

            // Lugar
            modelBuilder.Entity<Lugar>().ToTable("lugar");
            modelBuilder.Entity<Lugar>().HasKey(l => l.LugarId);
            modelBuilder.Entity<Lugar>().Property(l => l.LugarId).HasColumnName("id");
            modelBuilder.Entity<Lugar>().Property(l => l.Nombre).HasColumnName("nombre");

            // ConfiguracionTurno
            modelBuilder.Entity<ConfiguracionTurno>().ToTable("configuracion_turnos");
            modelBuilder.Entity<ConfiguracionTurno>().HasKey(ct => ct.TurnoId);
            modelBuilder.Entity<ConfiguracionTurno>().Property(ct => ct.TurnoId).HasColumnName("id");
            modelBuilder.Entity<ConfiguracionTurno>().Property(ct => ct.FechaInicio).HasColumnName("fecha_inicio");
            modelBuilder.Entity<ConfiguracionTurno>().Property(ct => ct.FechaFin).HasColumnName("fecha_fin");
            modelBuilder.Entity<ConfiguracionTurno>().Property(ct => ct.HorariosId).HasColumnName("horarios_id");
            modelBuilder.Entity<ConfiguracionTurno>().Property(ct => ct.CantidadEstaciones).HasColumnName("cantidad_estaciones");
            modelBuilder.Entity<ConfiguracionTurno>().Property(ct => ct.DuracionMinutos).HasColumnName("duracion_minutos");
            modelBuilder.Entity<ConfiguracionTurno>().Property(ct => ct.AunAceptaCitas).HasColumnName("aun_acepta_citas");

            // Relación con Horario
            modelBuilder.Entity<ConfiguracionTurno>()
                .HasOne(ct => ct.Horario)
                .WithMany(h => h.ConfiguracionesTurnos)
                .HasForeignKey(ct => ct.HorariosId);

            // Relación con FranjasHorario
            modelBuilder.Entity<ConfiguracionTurno>()
                .HasMany(ct => ct.Franjas)
                .WithOne(f => f.ConfiguracionTurno)
                .HasForeignKey(f => f.ConfiguracionTurnoId)
                .OnDelete(DeleteBehavior.Cascade);

            // FranjaHorario
            modelBuilder.Entity<FranjaHorario>().ToTable("FranjaHorario");
            modelBuilder.Entity<FranjaHorario>().HasKey(f => f.FranjaId);
            modelBuilder.Entity<FranjaHorario>().Property(f => f.FranjaId).HasColumnName("Id");
            modelBuilder.Entity<FranjaHorario>().Property(f => f.ConfiguracionTurnoId).HasColumnName("ConfiguracionTurnoId");
            modelBuilder.Entity<FranjaHorario>().Property(f => f.HoraInicio).HasColumnName("HoraInicio").IsRequired();
            modelBuilder.Entity<FranjaHorario>().Property(f => f.HoraFin).HasColumnName("HoraFin").IsRequired();

            // Cita
            modelBuilder.Entity<Cita>().ToTable("cita");
            modelBuilder.Entity<Cita>().HasKey(c => c.IdCita);
            modelBuilder.Entity<Cita>().Property(c => c.IdCita).HasColumnName("id_cita");
            modelBuilder.Entity<Cita>().Property(c => c.IdUsuario).HasColumnName("id_usuario");
            modelBuilder.Entity<Cita>().Property(c => c.TurnoId).HasColumnName("turno_id");
            modelBuilder.Entity<Cita>().Property(c => c.LugarId).HasColumnName("lugar_id");
            modelBuilder.Entity<Cita>().Property(c => c.ServicioId).HasColumnName("servicio_id");
            modelBuilder.Entity<Cita>().Property(c => c.Estado).HasColumnName("estado");
            modelBuilder.Entity<Cita>().Property(c => c.RowVersion).HasColumnName("row_version");
            modelBuilder.Entity<Cita>().Property(c => c.FechaCita).HasColumnName("fecha_cita");
            modelBuilder.Entity<Cita>().Property(c => c.FranjaId).HasColumnName("FranjaId");

            // Relaciones Cita
            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Citas)
                .HasForeignKey(c => c.IdUsuario);

            modelBuilder.Entity<Cita>()
           .HasOne(c => c.FranjaHorario)
           .WithMany()
           .HasForeignKey(c => c.FranjaId)
           .OnDelete(DeleteBehavior.NoAction);

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

            // Conversión de Enum a String
            modelBuilder.Entity<Cita>().Property(c => c.Estado).HasConversion<string>();

            // RowVersion
            modelBuilder.Entity<Cita>().Property(c => c.RowVersion).IsRowVersion();
        }
    }
}