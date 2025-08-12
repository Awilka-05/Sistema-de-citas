using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaGestionCitas.Domain.Entities;

namespace SistemaGestionCitas.Infrastructure.Persistence.BdContext
{
    public class SistemaCitasDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<ConfiguracionTurno> ConfiguracionesTurno { get; set; }
        public DbSet<SlotHorario> SlotsHorarios { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<LogSistema> LogsSistema { get; set; }
        public DbSet<CorreoPendiente> CorreosPendientes { get; set; }
        public DbSet<AuditoriaCita> AuditoriasCita { get; set; }

        public SistemaCitasDbContext(DbContextOptions<SistemaCitasDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("usuario");
                entity.HasKey(e => e.IdUsuario);
                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario").ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Apellido).HasColumnName("apellido").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Correo).HasColumnName("correo").HasMaxLength(100).IsRequired();
                entity.HasIndex(e => e.Correo).IsUnique();
                entity.Property(e => e.Telefono).HasColumnName("telefono").HasMaxLength(15);
                entity.Property(e => e.Cedula).HasColumnName("cedula").HasMaxLength(13).IsRequired();
                entity.HasIndex(e => e.Cedula).IsUnique();
                entity.Property(e => e.Contrasena).HasColumnName("contrasena").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Rol).HasColumnName("rol").HasMaxLength(20).HasDefaultValue("usuario");
                entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro").HasDefaultValueSql("getdate()");
                entity.Property(e => e.Activo).HasColumnName("activo").HasDefaultValue(true);
            });

            modelBuilder.Entity<Servicio>(entity =>
            {
                entity.ToTable("servicio");
                entity.HasKey(e => e.IdServicio);
                entity.Property(e => e.IdServicio).HasColumnName("id_servicio").ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Descripcion).HasColumnName("descripcion").HasMaxLength(255);
                entity.Property(e => e.DuracionMinutos).HasColumnName("duracion_minutos").HasDefaultValue(15);
                entity.Property(e => e.Activo).HasColumnName("activo").HasDefaultValue(true);
            });

            modelBuilder.Entity<ConfiguracionTurno>(entity =>
            {
                entity.ToTable("configuracion_turno");
                entity.HasKey(e => e.IdConfiguracion);
                entity.Property(e => e.IdConfiguracion).HasColumnName("id_configuracion").ValueGeneratedOnAdd();
                entity.Property(e => e.Fecha).HasColumnName("fecha").IsRequired();
                entity.Property(e => e.Turno).HasColumnName("turno").HasMaxLength(20).IsRequired(); 
                entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio").IsRequired();
                entity.Property(e => e.HoraFin).HasColumnName("hora_fin").IsRequired();
                entity.Property(e => e.IdServicio).HasColumnName("id_servicio").IsRequired();
                entity.Property(e => e.EstacionesDisponibles).HasColumnName("estaciones_disponibles").IsRequired();
                entity.Property(e => e.DuracionSlotMinutos).HasColumnName("duracion_slot_minutos").HasDefaultValue(15);
                entity.Property(e => e.Activo).HasColumnName("activo").HasDefaultValue(true);
                entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("getdate()");

                entity.HasOne<Servicio>()
                    .WithMany()
                    .HasForeignKey(e => e.IdServicio)
                    .IsRequired();

                entity.HasIndex(e => new { e.Fecha, e.Turno, e.IdServicio }).IsUnique();
            });

            modelBuilder.Entity<SlotHorario>(entity =>
            {
                entity.ToTable("slot_horario");
                entity.HasKey(e => e.IdSlot);
                entity.Property(e => e.IdSlot).HasColumnName("id_slot").ValueGeneratedOnAdd();
                entity.Property(e => e.IdConfiguracion).HasColumnName("id_configuracion").IsRequired();
                entity.Property(e => e.Fecha).HasColumnName("fecha").IsRequired();
                entity.Property(e => e.Hora).HasColumnName("hora").IsRequired();
                entity.Property(e => e.CupoMaximo).HasColumnName("cupo_maximo").IsRequired();
                entity.Property(e => e.CupoOcupado).HasColumnName("cupo_ocupado").HasDefaultValue(0);
                entity.Property(e => e.Activo).HasColumnName("activo").HasDefaultValue(true);

                entity.HasOne<ConfiguracionTurno>()
                    .WithMany()
                    .HasForeignKey(e => e.IdConfiguracion)
                    .IsRequired();

                entity.HasIndex(e => new { e.IdConfiguracion, e.Hora }).IsUnique();
            });

            modelBuilder.Entity<Cita>(entity =>
            {
                entity.ToTable("cita");
                entity.HasKey(e => e.IdCita);
                entity.Property(e => e.IdCita).HasColumnName("id_cita").ValueGeneratedOnAdd();
                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario").IsRequired();
                entity.Property(e => e.IdSlot).HasColumnName("id_slot").IsRequired();
                entity.Property(e => e.CodigoCita).HasColumnName("codigo_cita").HasMaxLength(20).IsRequired();
                entity.HasIndex(e => e.CodigoCita).IsUnique();
                entity.Property(e => e.Estado).HasColumnName("estado").HasMaxLength(20).HasDefaultValue("pendiente");           
                entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("getdate()");
                entity.Property(e => e.FechaActualizacion).HasColumnName("fecha_actualizacion").HasDefaultValueSql("getdate()");
                entity.Property(e => e.Observaciones).HasColumnName("observaciones").HasMaxLength(500);

             
                entity.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(e => e.IdUsuario)
                    .IsRequired();

                entity.HasOne<SlotHorario>()
                    .WithMany()
                    .HasForeignKey(e => e.IdSlot)
                    .IsRequired();

                entity.HasIndex(e => new { e.IdUsuario, e.IdSlot }).IsUnique();
            });

            modelBuilder.Entity<LogSistema>(entity =>
            {
                entity.ToTable("log_sistema");
                entity.HasKey(e => e.IdLog);
                entity.Property(e => e.IdLog).HasColumnName("id_log").ValueGeneratedOnAdd();
                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
                entity.Property(e => e.Accion).HasColumnName("accion").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Detalle).HasColumnName("detalle");
                entity.Property(e => e.Tipo).HasColumnName("tipo").HasMaxLength(20).IsRequired();
                entity.Property(e => e.DireccionIp).HasColumnName("direccion_ip").HasMaxLength(45);
                entity.Property(e => e.UserAgent).HasColumnName("user_agent").HasMaxLength(500);
                entity.Property(e => e.Fecha).HasColumnName("fecha").HasDefaultValueSql("getdate()");

                entity.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(e => e.IdUsuario)
                    .IsRequired(false);
            });

            modelBuilder.Entity<CorreoPendiente>(entity =>
            {
                entity.ToTable("correo_pendiente");
                entity.HasKey(e => e.IdCorreo);
                entity.Property(e => e.IdCorreo).HasColumnName("id_correo").ValueGeneratedOnAdd();
                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario").IsRequired();
                entity.Property(e => e.IdCita).HasColumnName("id_cita");
                entity.Property(e => e.Destinatario).HasColumnName("destinatario").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Asunto).HasColumnName("asunto").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Cuerpo).HasColumnName("cuerpo").IsRequired();
                entity.Property(e => e.Tipo).HasColumnName("tipo").HasMaxLength(50).IsRequired();
                entity.Property(e => e.IntentosEnvio).HasColumnName("intentos_envio").HasDefaultValue(0);
                entity.Property(e => e.MaxIntentos).HasColumnName("max_intentos").HasDefaultValue(3);
                entity.Property(e => e.Enviado).HasColumnName("enviado").HasDefaultValue(false);
                entity.Property(e => e.FechaCreacion).HasColumnName("fecha_creacion").HasDefaultValueSql("getdate()");
                entity.Property(e => e.FechaEnvio).HasColumnName("fecha_envio");
                entity.Property(e => e.ErrorMensaje).HasColumnName("error_mensaje").HasMaxLength(500);

                entity.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(e => e.IdUsuario)
                    .IsRequired();

                entity.HasOne<Cita>()
                    .WithMany()
                    .HasForeignKey(e => e.IdCita)
                    .IsRequired(false);
            });

            modelBuilder.Entity<AuditoriaCita>(entity =>
            {
                entity.ToTable("auditoria_cita");
                entity.HasKey(e => e.IdAuditoria);
                entity.Property(e => e.IdAuditoria).HasColumnName("id_auditoria").ValueGeneratedOnAdd();
                entity.Property(e => e.IdCita).HasColumnName("id_cita").IsRequired();
                entity.Property(e => e.EstadoAnterior).HasColumnName("estado_anterior").HasMaxLength(20);
                entity.Property(e => e.EstadoNuevo).HasColumnName("estado_nuevo").HasMaxLength(20).IsRequired();
                entity.Property(e => e.IdUsuarioCambio).HasColumnName("id_usuario_cambio");
                entity.Property(e => e.Motivo).HasColumnName("motivo").HasMaxLength(255);
                entity.Property(e => e.FechaCambio).HasColumnName("fecha_cambio").HasDefaultValueSql("getdate()");

                entity.HasOne<Cita>()
                    .WithMany()
                    .HasForeignKey(e => e.IdCita)
                    .IsRequired();

                entity.HasOne<Usuario>()
                    .WithMany()
                    .HasForeignKey(e => e.IdUsuarioCambio)
                    .IsRequired(false);
            });
        }
    }

}

