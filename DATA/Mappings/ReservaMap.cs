using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SIRS.Domain.Models;

namespace SIRS.Data.Mappings
{
    public class ReservaMap : IEntityTypeConfiguration<Reserva>
    {
        public void Configure(EntityTypeBuilder<Reserva> builder)
        {
            builder.Property(r => r.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(r => r.Nombre)
                .HasColumnType("nvarchar(500)")
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(r => r.Observaciones)
                .HasColumnType("nvarchar(1000)")
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(r => r.FechaReserva)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(r => r.HoraInicio)
                .HasColumnType("time(7)")
                .IsRequired();

            builder.Property(r => r.TiempoTotal)
                .HasColumnType("int")
                .IsRequired();

            builder.Property(r => r.SalaId)
                .HasColumnName("SalaId")
                .IsRequired();

            builder.Property(r => r.UsuarioId)
                .HasColumnName("UsuarioId")
                .IsRequired();

            builder.HasKey(r => r.Id);
            builder.HasOne(r => r.Sala)
                .WithMany(s => s.Reservas)
                .HasForeignKey(r => r.SalaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Usuario)
                .WithMany(u => u.Reservas)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}