using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Data.Repositorio
{
    public class Reserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Autoincremental
        public int Id { get; set; }

        [MaxLength(500)]
        public string Nombre { get; set; }

        [MaxLength(1000)]
        public string Observaciones { get; set; }

        public DateTime? FechaReserva { get; set; }

        public TimeSpan? HoraInicio { get; set; }

        public int? TiempoTotal { get; set; }

        [ForeignKey("Sala")]
        public int SalaId { get; set; }
        public Sala Sala { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}