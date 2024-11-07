using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Data.Repositorio
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }  // No autoincremental

        [MaxLength(200)]
        [Required]
        public string Nombre { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}