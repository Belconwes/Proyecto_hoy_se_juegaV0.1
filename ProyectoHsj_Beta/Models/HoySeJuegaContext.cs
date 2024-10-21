using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProyectoHsj_Beta.Models;

public partial class HoySeJuegaContext : DbContext
{
    public HoySeJuegaContext()
    {
    }

    public HoySeJuegaContext(DbContextOptions<HoySeJuegaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccionRealizadum> AccionRealizada { get; set; }

    public virtual DbSet<Auditorium> Auditoria { get; set; }

    public virtual DbSet<Cancha> Canchas { get; set; }

    public virtual DbSet<EstadoReserva> EstadoReservas { get; set; }

    public virtual DbSet<HorarioDisponible> HorarioDisponibles { get; set; }

    public virtual DbSet<Notificacion> Notificacions { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Reporte> Reportes { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<TituloNotificacion> TituloNotificacions { get; set; }

    public virtual DbSet<TituloReporte> TituloReportes { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        //=> optionsBuilder.UseSqlServer("Server=(local); DataBase=Hsj; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccionRealizadum>(entity =>
        {
            entity.HasKey(e => e.IdAccionRealizada).HasName("PK__ACCION_R__2DC6E876AE3CA4B6");

            entity.ToTable("ACCION_REALIZADA");

            entity.Property(e => e.IdAccionRealizada).HasColumnName("ID_accion_realizada");
            entity.Property(e => e.TituloAccionRealizada)
                .HasMaxLength(70)
                .HasColumnName("Titulo_Accion_Realizada");
        });

        modelBuilder.Entity<Auditorium>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("PK__AUDITORI__F6FFFB8CE3101847");

            entity.ToTable("AUDITORIA");

            entity.Property(e => e.IdAuditoria).HasColumnName("ID_auditoria");
            entity.Property(e => e.DescripcionDeAccion).HasColumnName("Descripcion_De_Accion");
            entity.Property(e => e.FechaAuditoria)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Auditoria");
            entity.Property(e => e.IdAccionRealizada).HasColumnName("ID_accion_realizada");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_usuario");

            entity.HasOne(d => d.IdAccionRealizadaNavigation).WithMany(p => p.Auditoria)
                .HasForeignKey(d => d.IdAccionRealizada)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AUDITORIA__ID_ac__4CA06362");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Auditoria)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AUDITORIA__ID_us__4D94879B");
        });

        modelBuilder.Entity<Cancha>(entity =>
        {
            entity.HasKey(e => e.IdCancha).HasName("PK__CANCHA__A2D3DBCF8F513BBF");

            entity.ToTable("CANCHA");

            entity.Property(e => e.IdCancha).HasColumnName("ID_cancha");
            entity.Property(e => e.NombreCancha)
                .HasMaxLength(70)
                .HasColumnName("Nombre_Cancha");
            entity.Property(e => e.UbicacionCancha)
                .HasMaxLength(120)
                .HasColumnName("Ubicacion_Cancha");
        });

        modelBuilder.Entity<EstadoReserva>(entity =>
        {
            entity.HasKey(e => e.IdEstadoReserva).HasName("PK__ESTADO_R__746A84F0EE561664");

            entity.ToTable("ESTADO_RESERVA");

            entity.Property(e => e.IdEstadoReserva).HasColumnName("ID_estado_reserva");
            entity.Property(e => e.NombreEstadoReserva)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("Nombre_Estado_Reserva");
        });

        modelBuilder.Entity<HorarioDisponible>(entity =>
        {
            entity.HasKey(e => e.IdHorarioDisponible).HasName("PK__HORARIO___7D5601B788E66056");

            entity.ToTable("HORARIO_DISPONIBLE");

            entity.Property(e => e.IdHorarioDisponible).HasColumnName("ID_horario_disponible");
            entity.Property(e => e.DisponibleHorario)
                .HasDefaultValue(true)
                .HasColumnName("Disponible_Horario");
            entity.Property(e => e.FechaHorario)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Fecha_Horario");
            entity.Property(e => e.HoraFin).HasColumnName("Hora_Fin");
            entity.Property(e => e.HoraInicio).HasColumnName("Hora_Inicio");
            entity.Property(e => e.IdCancha).HasColumnName("ID_cancha");

            entity.HasOne(d => d.IdCanchaNavigation).WithMany(p => p.HorarioDisponibles)
                .HasForeignKey(d => d.IdCancha)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HORARIO_D__ID_ca__5441852A");
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.HasKey(e => e.IdNotificacion).HasName("PK__NOTIFICA__99BC7E5EC04B01F2");

            entity.ToTable("NOTIFICACION");

            entity.Property(e => e.IdNotificacion).HasColumnName("ID_notificacion");
            entity.Property(e => e.FechaEnvioNotificacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Envio_Notificacion");
            entity.Property(e => e.IdReserva).HasColumnName("ID_reserva");
            entity.Property(e => e.IdTituloNotificacion).HasColumnName("ID_titulo_notificacion");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_usuario");
            entity.Property(e => e.MensajeNotificacion).HasColumnName("Mensaje_Notificacion");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.Notificacions)
                .HasForeignKey(d => d.IdReserva)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NOTIFICAC__ID_re__6754599E");

            entity.HasOne(d => d.IdTituloNotificacionNavigation).WithMany(p => p.Notificacions)
                .HasForeignKey(d => d.IdTituloNotificacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NOTIFICAC__ID_ti__656C112C");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Notificacions)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NOTIFICAC__ID_us__66603565");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PK__PAGO__808903EC47A081C7");

            entity.ToTable("PAGO");

            entity.Property(e => e.IdPago).HasColumnName("ID_pago");
            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Pago");
            entity.Property(e => e.IdReserva).HasColumnName("ID_reserva");
            entity.Property(e => e.MontoPago)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Monto_Pago");

            entity.HasOne(d => d.IdReservaNavigation).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.IdReserva)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PAGO__ID_reserva__5FB337D6");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermiso).HasName("PK__PERMISO__74B1E219FA556AF3");

            entity.ToTable("PERMISO");

            entity.Property(e => e.IdPermiso).HasColumnName("ID_permiso");
            entity.Property(e => e.NombrePermiso)
                .HasMaxLength(50)
                .HasColumnName("Nombre_Permiso");
        });

        modelBuilder.Entity<Reporte>(entity =>
        {
            entity.HasKey(e => e.IdReporte).HasName("PK__REPORTE__41AEEB64E2FCCC33");

            entity.ToTable("REPORTE");

            entity.Property(e => e.IdReporte).HasColumnName("ID_reporte");
            entity.Property(e => e.AnioReporte).HasColumnName("Anio_Reporte");
            entity.Property(e => e.DescripcionReporte)
                .IsUnicode(false)
                .HasColumnName("Descripcion_Reporte");
            entity.Property(e => e.FechaDeReporte)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_De_Reporte");
            entity.Property(e => e.IdTituloReporte).HasColumnName("ID_titulo_reporte");
            entity.Property(e => e.MesReporte).HasColumnName("Mes_Reporte");
            entity.Property(e => e.ReservasRealizadasReporte).HasColumnName("Reservas_Realizadas_Reporte");
            entity.Property(e => e.UsuariosRegistradosReporte).HasColumnName("Usuarios_Registrados_Reporte");

            entity.HasOne(d => d.IdTituloReporteNavigation).WithMany(p => p.Reportes)
                .HasForeignKey(d => d.IdTituloReporte)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__REPORTE__ID_titu__3A81B327");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.IdReserva).HasName("PK__RESERVA__CD692CB066E0B8CB");

            entity.ToTable("RESERVA");

            entity.Property(e => e.IdReserva).HasColumnName("ID_reserva");
            entity.Property(e => e.FechaReserva)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Reserva");
            entity.Property(e => e.IdEstadoReserva).HasColumnName("ID_estado_reserva");
            entity.Property(e => e.IdHorarioDisponible).HasColumnName("ID_horario_disponible");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_usuario");

            entity.HasOne(d => d.IdEstadoReservaNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdEstadoReserva)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RESERVA__ID_esta__59FA5E80");

            entity.HasOne(d => d.IdHorarioDisponibleNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdHorarioDisponible)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RESERVA__ID_hora__5BE2A6F2");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RESERVA__ID_usua__5AEE82B9");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__ROL__182A5412E3FEA872");

            entity.ToTable("ROL");

            entity.Property(e => e.IdRol).HasColumnName("ID_rol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(50)
                .HasColumnName("Nombre_Rol");

            entity.HasMany(d => d.IdPermisos).WithMany(p => p.IdRols)
                .UsingEntity<Dictionary<string, object>>(
                    "PermisoRol",
                    r => r.HasOne<Permiso>().WithMany()
                        .HasForeignKey("IdPermiso")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PERMISO_R__ID_pe__45F365D3"),
                    l => l.HasOne<Rol>().WithMany()
                        .HasForeignKey("IdRol")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PERMISO_R__ID_ro__46E78A0C"),
                    j =>
                    {
                        j.HasKey("IdRol", "IdPermiso").HasName("PK__PERMISO___9F614A336556D35C");
                        j.ToTable("PERMISO_ROL");
                        j.IndexerProperty<int>("IdRol").HasColumnName("ID_rol");
                        j.IndexerProperty<int>("IdPermiso").HasColumnName("ID_permiso");
                    });
        });

        modelBuilder.Entity<TituloNotificacion>(entity =>
        {
            entity.HasKey(e => e.IdTituloNotificacion).HasName("PK__TITULO_N__DA36445EB454B7E0");

            entity.ToTable("TITULO_NOTIFICACION");

            entity.Property(e => e.IdTituloNotificacion).HasColumnName("ID_titulo_notificacion");
            entity.Property(e => e.TituloNotificacion1)
                .HasMaxLength(80)
                .HasColumnName("Titulo_Notificacion");
        });

        modelBuilder.Entity<TituloReporte>(entity =>
        {
            entity.HasKey(e => e.IdTituloReporte).HasName("PK__TITULO_R__5F9C0CCDB6AEF5F9");

            entity.ToTable("TITULO_REPORTE");

            entity.Property(e => e.IdTituloReporte).HasColumnName("ID_titulo_reporte");
            entity.Property(e => e.TituloReporte1)
                .HasMaxLength(80)
                .HasColumnName("Titulo_Reporte");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__USUARIO__DF3D425202CA1D01");

            entity.ToTable("USUARIO");

            entity.HasIndex(e => e.CorreoUsuario, "UQ__USUARIO__A7126311EFE6490D").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("ID_usuario");
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.ApellidoUsuario)
                .HasMaxLength(30)
                .HasColumnName("Apellido_Usuario");
            entity.Property(e => e.ContraseniaUsuario)
                .HasMaxLength(255)
                .HasColumnName("Contrasenia_Usuario");
            entity.Property(e => e.CorreoUsuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Correo_Usuario");
            entity.Property(e => e.EmailConfirmationToken).HasMaxLength(255);
            entity.Property(e => e.IdRol).HasColumnName("ID_rol");
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(30)
                .HasColumnName("Nombre_Usuario");
            entity.Property(e => e.PasswordResetTokenExpiry).HasColumnType("datetime");
            entity.Property(e => e.TelefonoUsuario).HasColumnName("Telefono_Usuario");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__USUARIO__ID_rol__412EB0B6");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
