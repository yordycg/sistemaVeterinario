using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace sistemaVeterinario.Models;

public partial class SistemaVeterinarioContext : DbContext
{
    public SistemaVeterinarioContext()
    {
    }

    public SistemaVeterinarioContext(DbContextOptions<SistemaVeterinarioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Consulta> Consultas { get; set; }

    public virtual DbSet<Especy> Especies { get; set; }

    public virtual DbSet<EstadoConsulta> EstadoConsultas { get; set; }

    public virtual DbSet<EstadoUsuario> EstadoUsuarios { get; set; }

    public virtual DbSet<Mascota> Mascotas { get; set; }

    public virtual DbSet<Raza> Razas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tratamiento> Tratamientos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=sistema_veterinario_db;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__clientes__677F38F5448FD6C5");

            entity.ToTable("clientes");

            entity.HasIndex(e => e.Email, "UQ__clientes__AB6E6164286AA167").IsUnique();

            entity.HasIndex(e => e.Run, "UQ__clientes__C2B74E6CB8485CC1").IsUnique();

            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Run)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("run");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.HasKey(e => e.IdConsulta).HasName("PK__consulta__6F53588BC63A384F");

            entity.ToTable("consultas");

            entity.Property(e => e.IdConsulta).HasColumnName("id_consulta");
            entity.Property(e => e.Diagnostico)
                .HasColumnType("text")
                .HasColumnName("diagnostico");
            entity.Property(e => e.FechaConsulta).HasColumnName("fecha_consulta");
            entity.Property(e => e.IdEstadoConsulta).HasColumnName("id_estado_consulta");
            entity.Property(e => e.IdMascota).HasColumnName("id_mascota");
            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Motivo)
                .HasColumnType("text")
                .HasColumnName("motivo");

            entity.HasOne(d => d.IdEstadoConsultaNavigation).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.IdEstadoConsulta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__consultas__id_es__534D60F1");

            entity.HasOne(d => d.IdMascotaNavigation).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.IdMascota)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__consultas__id_ma__5165187F");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__consultas__id_us__52593CB8");
        });

        modelBuilder.Entity<Especy>(entity =>
        {
            entity.HasKey(e => e.IdEspecie).HasName("PK__especies__96DDB0B982955345");

            entity.ToTable("especies");

            entity.Property(e => e.IdEspecie).HasColumnName("id_especie");
            entity.Property(e => e.NombreEspecie)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nombre_especie");
        });

        modelBuilder.Entity<EstadoConsulta>(entity =>
        {
            entity.HasKey(e => e.IdEstadoConsulta).HasName("PK__estado_c__19A53235F51D74B3");

            entity.ToTable("estado_consultas");

            entity.Property(e => e.IdEstadoConsulta).HasColumnName("id_estado_consulta");
            entity.Property(e => e.NombreEstado)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("nombre_estado");
        });

        modelBuilder.Entity<EstadoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdEstadoUsuario).HasName("PK__estado_u__CEFB9B89DD0EA143");

            entity.ToTable("estado_usuarios");

            entity.Property(e => e.IdEstadoUsuario).HasColumnName("id_estado_usuario");
            entity.Property(e => e.NombreEstado)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("nombre_estado");
        });

        modelBuilder.Entity<Mascota>(entity =>
        {
            entity.HasKey(e => e.IdMascota).HasName("PK__mascotas__6F0373524828A893");

            entity.ToTable("mascotas");

            entity.Property(e => e.IdMascota).HasColumnName("id_mascota");
            entity.Property(e => e.Edad).HasColumnName("edad");
            entity.Property(e => e.IdCliente).HasColumnName("id_cliente");
            entity.Property(e => e.IdRaza).HasColumnName("id_raza");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Sexo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("sexo");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Mascota)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mascotas__id_cli__4CA06362");

            entity.HasOne(d => d.IdRazaNavigation).WithMany(p => p.Mascota)
                .HasForeignKey(d => d.IdRaza)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mascotas__id_raz__4E88ABD4");
        });

        modelBuilder.Entity<Raza>(entity =>
        {
            entity.HasKey(e => e.IdRaza).HasName("PK__razas__084F250A6A7054C8");

            entity.ToTable("razas");

            entity.Property(e => e.IdRaza).HasColumnName("id_raza");
            entity.Property(e => e.IdEspecie).HasColumnName("id_especie");
            entity.Property(e => e.NombreRaza)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("nombre_raza");

            entity.HasOne(d => d.IdEspecieNavigation).WithMany(p => p.Razas)
                .HasForeignKey(d => d.IdEspecie)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_razas_especies");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__roles__6ABCB5E0E52DCD0F");

            entity.ToTable("roles");

            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("nombre_rol");
        });

        modelBuilder.Entity<Tratamiento>(entity =>
        {
            entity.HasKey(e => e.IdTratamiento).HasName("PK__tratamie__C8825F4CB6B1F22C");

            entity.ToTable("tratamientos");

            entity.Property(e => e.IdTratamiento).HasColumnName("id_tratamiento");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdConsulta).HasColumnName("id_consulta");
            entity.Property(e => e.Medicamento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("medicamento");

            entity.HasOne(d => d.IdConsultaNavigation).WithMany(p => p.Tratamientos)
                .HasForeignKey(d => d.IdConsulta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tratamien__id_co__5629CD9C");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__usuarios__4E3E04AD4CB7E388");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "UQ__usuarios__AB6E616446B1A300").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IdEstadoUsuario).HasColumnName("id_estado_usuario");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");

            entity.HasOne(d => d.IdEstadoUsuarioNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdEstadoUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__usuarios__id_est__47DBAE45");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__usuarios__id_rol__46E78A0C");
        });

        // TODO: Migrar los datos iniciales del archivo database/data.sql a este método de siembra.
        // Por ejemplo: Especies, Razas, etc.

        modelBuilder.Entity<EstadoUsuario>().HasData(
            new EstadoUsuario { IdEstadoUsuario = 1, NombreEstado = "Activo" },
            new EstadoUsuario { IdEstadoUsuario = 2, NombreEstado = "Inactivo" },
            new EstadoUsuario { IdEstadoUsuario = 3, NombreEstado = "Pendiente de Activación" }, // confirmacion de un correo.
            new EstadoUsuario { IdEstadoUsuario = 4, NombreEstado = "Bloqueado" }, // por admin.
            new EstadoUsuario { IdEstadoUsuario = 5, NombreEstado = "Suspendido" } // intentos fallidos al ingresar al sistema.
        );

        modelBuilder.Entity<Especy>().HasData(
            new Especy { IdEspecie = 1, NombreEspecie = "Perro" },
            new Especy { IdEspecie = 2, NombreEspecie = "Gato" },
            new Especy { IdEspecie = 3, NombreEspecie = "Hámster" },
            new Especy { IdEspecie = 4, NombreEspecie = "Conejo" },
            new Especy { IdEspecie = 5, NombreEspecie = "Ave" },
            new Especy { IdEspecie = 6, NombreEspecie = "Tortuga" },
            new Especy { IdEspecie = 7, NombreEspecie = "Pez" }
        );

        modelBuilder.Entity<Raza>().HasData(
            new Raza { IdRaza = 1, IdEspecie = 1, NombreRaza = "Labrador" },
            new Raza { IdRaza = 2, IdEspecie = 1, NombreRaza = "Poodle" },
            new Raza { IdRaza = 8, IdEspecie = 1, NombreRaza = "Pastor Alemán" }, 
            new Raza { IdRaza = 3, IdEspecie = 2, NombreRaza = "Siamés" },
            new Raza { IdRaza = 4, IdEspecie = 2, NombreRaza = "Persa" },
            new Raza { IdRaza = 9, IdEspecie = 2, NombreRaza = "Maine Coon" }, 
            new Raza { IdRaza = 5, IdEspecie = 3, NombreRaza = "Dorado" },
            new Raza { IdRaza = 6, IdEspecie = 4, NombreRaza = "Cabeza de León" },
            new Raza { IdRaza = 7, IdEspecie = 5, NombreRaza = "Canario" },
            new Raza { IdRaza = 10, IdEspecie = 6, NombreRaza = "Tortuga de Tierra" }, 
            new Raza { IdRaza = 11, IdEspecie = 7, NombreRaza = "Goldfish" } 
        );

        modelBuilder.Entity<Role>().HasData(
            new Role { IdRol = 1, NombreRol = "Admin" },
            new Role { IdRol = 2, NombreRol = "Veterinari@" },
            new Role { IdRol = 3, NombreRol = "Recepcionista" }
        );

        modelBuilder.Entity<EstadoConsulta>().HasData(
            new EstadoConsulta { IdEstadoConsulta = 1, NombreEstado = "Pendiente" },
            new EstadoConsulta { IdEstadoConsulta = 2, NombreEstado = "En Progreso" },
            new EstadoConsulta { IdEstadoConsulta = 3, NombreEstado = "Finalizada" }
        );

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
