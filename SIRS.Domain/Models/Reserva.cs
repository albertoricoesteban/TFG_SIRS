using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Domain.Models
{
    public class Reserva
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Observaciones { get; set; }

        public DateTime? FechaReserva { get; set; }

        public TimeSpan? HoraInicio { get; set; }

        public int? TiempoTotal { get; set; }

        public int SalaId { get; set; }
        public Sala Sala { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public bool? Aprobada { get; set; }

        public DateTime? FechaBaja { get; set; }
    }
}