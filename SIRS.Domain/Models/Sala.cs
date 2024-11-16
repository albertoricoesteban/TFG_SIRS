using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Domain.Models
{
    public class Sala
    {
       
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public string NombreCorto { get; set; }

        public int? Capacidad { get; set; }

        public int EstadoSalaId { get; set; }
        public EstadoSala EstadoSala { get; set; }
        public int EdificioId { get; set; }
        public Edificio Edificio { get; set; }

        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();


    }
}