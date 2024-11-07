using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SIRS.Domain.Models;

namespace SIRS.Data.Mappings
{
    public class EstadoSalaMap : IEntityTypeConfiguration<EstadoSala>
    {
        public void Configure(EntityTypeBuilder<EstadoSala> builder)
        {
            builder.Property(e => e.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Descripcion)
                .HasColumnType("nvarchar(250)")
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}