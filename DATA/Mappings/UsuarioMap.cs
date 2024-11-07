using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SIRS.Domain.Models;

namespace SIRS.Data.Mappings
{

    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.Property(u => u.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Username)
                .HasColumnType("nvarchar(10)")
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(u => u.Nombre)
                .HasColumnType("nvarchar(300)")
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(u => u.Apellido1)
                .HasColumnType("nvarchar(300)")
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(u => u.Apellido2)
                .HasColumnType("nvarchar(300)")
                .HasMaxLength(300)
                .IsRequired(false);

            builder.Property(u => u.Password)
                .HasColumnType("varchar(255)")
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnType("nvarchar(250)")
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(u => u.FechaRegistro)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(u => u.RolId)
                .HasColumnName("RolId")
                .IsRequired();

            builder.HasKey(u => u.Id);
            builder.HasOne(u => u.Rol)
                .WithMany(r => r.Usuarios)
                .HasForeignKey(u => u.RolId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}