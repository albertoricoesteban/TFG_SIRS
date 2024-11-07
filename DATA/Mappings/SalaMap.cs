using SIRS.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SIRS.Domain.Models;


namespace SIRS.Data.Mappings
{
    public class SalaMap : IEntityTypeConfiguration<Sala>
    {
        public void Configure(EntityTypeBuilder<Sala> builder)
        {
            builder.Property(s => s.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(s => s.Descripcion)
                .HasColumnType("nvarchar(200)")
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(s => s.NombreCorto)
                .HasColumnType("nvarchar(200)")
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(s => s.Capacidad)
                .HasColumnType("int")
                .IsRequired(false);

            builder.Property(s => s.EstadoSalaId)
                .HasColumnName("EstadoSalaId")
                .IsRequired();

            builder.Property(s => s.EdificioId)
                .HasColumnName("EdificioId")
                .IsRequired();

            builder.HasKey(s => s.Id);
            builder.HasOne(s => s.EstadoSala)
                .WithMany(e => e.Salas)
                .HasForeignKey(s => s.EstadoSalaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Edificio)
                .WithMany(e => e.Salas)
                .HasForeignKey(s => s.EdificioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}