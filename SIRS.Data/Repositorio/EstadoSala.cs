using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Data.Repositorio
{
    public class EstadoSala
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Autoincremental
        public int Id { get; set; }

        [MaxLength(250)]
        public string Descripcion { get; set; }

        public ICollection<Sala> Salas { get; set; }
    }
}