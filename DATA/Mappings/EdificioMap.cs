using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SIRS.Domain.Models;

namespace SIRS.Data.Mappings
{
    public class EdificioMap : IEntityTypeConfiguration<Edificio>
    {
        public void Configure(EntityTypeBuilder<Edificio> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Descripcion)
                .HasColumnType("nvarchar(500)")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(e => e.Direccion)
                .HasColumnType("nvarchar(500)")
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(e => e.Latitud)
                .HasColumnType("decimal(18, 4)")
                .HasPrecision(18, 4)
                .IsRequired(false);

            builder.Property(e => e.Longitud)
                .HasColumnType("decimal(10, 6)")
                .HasPrecision(10, 6)
                .IsRequired(false);

            builder.HasKey(e => e.Id);
        }
    }
}