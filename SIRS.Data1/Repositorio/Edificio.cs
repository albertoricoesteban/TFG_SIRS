using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIRS.Data.Repositorio
{
    public class Edificio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Autoincremental
        public int Id { get; set; }

        [MaxLength(500)]
        public string Descripcion { get; set; }

        [MaxLength(500)]
        public string Direccion { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? Latitud { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal? Longitud { get; set; }

        public ICollection<Sala> Salas { get; set; }

    }
}