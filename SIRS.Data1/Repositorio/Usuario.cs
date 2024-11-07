using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Data.Repositorio
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // Autoincremental
        public int Id { get; set; }

        [MaxLength(10)]
        [Required]
        public string Username { get; set; }

        [MaxLength(300)]
        [Required]
        public string Nombre { get; set; }

        [MaxLength(300)]
        [Required]
        public string Apellido1 { get; set; }

        [MaxLength(300)]
        public string Apellido2 { get; set; }

        [MaxLength(255)]
        [Required]
        public string Password { get; set; }

        [MaxLength(250)]
        [Required]
        public string Email { get; set; }

        public DateTime FechaRegistro { get; set; }

        [ForeignKey("Rol")]
        public int RolId { get; set; }
        public Rol Rol { get; set; }

        public ICollection<Reserva> Reservas { get; set; }
    }
}