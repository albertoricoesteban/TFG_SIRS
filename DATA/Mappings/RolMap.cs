using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SIRS.Domain.Models;

namespace SIRS.Data.Mappings
{
    public class RolMap : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.Property(r => r.Id)
                .HasColumnName("Id");

            builder.Property(r => r.Nombre)
                .HasColumnType("nvarchar(200)")
                .HasMaxLength(200)
                .IsRequired();

            builder.HasKey(r => r.Id);
        }
    }
}