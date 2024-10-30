using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Data.Repositorio
{
    public class Sala
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Autoincremental
        public int Id { get; set; }

        [MaxLength(200)]
        public string Descripcion { get; set; }

        [MaxLength(200)]
        public string NombreCorto { get; set; }

        public int? Capacidad { get; set; }

        [ForeignKey("EstadoSala")]
        public int EstadoSalaId { get; set; }
        public EstadoSala EstadoSala { get; set; }

        [ForeignKey("Edificio")]
        public int EdificioId { get; set; }
        public Edificio Edificio { get; set; }

        public ICollection<Reserva> Reservas { get; set; }

    }
}