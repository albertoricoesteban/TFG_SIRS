using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Domain.Models
{
    public class EstadoSala
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } 
    }
}