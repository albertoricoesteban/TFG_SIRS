using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Domain.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }

        public string Apellido2 { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public DateTime FechaRegistro { get; set; }

        public int RolId { get; set; }
        public Rol Rol { get; set; }

        public ICollection<Reserva> Reservas { get; set; }
    }
}