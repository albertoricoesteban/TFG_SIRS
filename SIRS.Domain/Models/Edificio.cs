using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SIRS.Domain.Models
{
    public class Edificio
    {
        
        public int Id { get; set; }
        public string Descripcion { get; set; }

        public string Direccion { get; set; }

        public decimal? Latitud { get; set; }

        public decimal? Longitud { get; set; }

        public ICollection<Sala> Salas { get; set; } = new List<Sala>(); // Inicializamos como una lista vacía si no hay salas

    }
}