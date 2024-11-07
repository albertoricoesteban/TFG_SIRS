using Microsoft.EntityFrameworkCore;
using SIRS.Data.Repositorio;

namespace SIRS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Edificio> Edificios { get; set; }
        public DbSet<EstadoSala> EstadoSalas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de Edificio
            modelBuilder.Entity<Edificio>()
                .Property(e => e.Descripcion)
                .HasMaxLength(500);

            modelBuilder.Entity<Edificio>()
                .Property(e => e.Direccion)
                .HasMaxLength(500);

            modelBuilder.Entity<Edificio>()
                .Property(e => e.Latitud)
                .HasPrecision(18,4);

            modelBuilder.Entity<Edificio>()
                .Property(e => e.Longitud)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Edificio>()
                .HasMany(e => e.Salas)
                .WithOne(s => s.Edificio)
                .HasForeignKey(s => s.EdificioId);

            // Configuración de EstadoSala
            modelBuilder.Entity<EstadoSala>()
                .Property(e => e.Descripcion)
                .HasMaxLength(250);

            modelBuilder.Entity<EstadoSala>()
                .HasMany(e => e.Salas)
                .WithOne(s => s.EstadoSala)
                .HasForeignKey(s => s.EstadoSalaId);

            // Configuración de Sala
            modelBuilder.Entity<Sala>()
                .Property(s => s.Descripcion)
                .HasMaxLength(200);

            modelBuilder.Entity<Sala>()
                .Property(s => s.NombreCorto)
                .HasMaxLength(200);

            modelBuilder.Entity<Sala>()
                .HasMany(s => s.Reservas)
                .WithOne(r => r.Sala)
                .HasForeignKey(r => r.SalaId);

            // Configuración de Reserva
            modelBuilder.Entity<Reserva>()
                .Property(r => r.Nombre)
                .HasMaxLength(500);

            modelBuilder.Entity<Reserva>()
                .Property(r => r.Observaciones)
                .HasMaxLength(1000);

            // Configuración de Rol
            modelBuilder.Entity<Rol>()
                .Property(r => r.Nombre)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Rol>()
                .HasMany(r => r.Usuarios)
                .WithOne(u => u.Rol)
                .HasForeignKey(u => u.RolId);

            // Configuración de Usuario
            modelBuilder.Entity<Usuario>()
                .Property(u => u.Username)
                .HasMaxLength(10)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Nombre)
                .HasMaxLength(300)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Apellido1)
                .HasMaxLength(300)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Apellido2)
                .HasMaxLength(300);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Password)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Email)
                .HasMaxLength(250)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Reservas)
                .WithOne(r => r.Usuario)
                .HasForeignKey(r => r.UsuarioId);
        }
    }
}