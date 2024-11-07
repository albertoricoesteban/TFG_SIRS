using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Domain.Models
{
    public class Rol
    {
        public int Id { get; set; }  // No autoincremental

        public string Nombre { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }
    }
}