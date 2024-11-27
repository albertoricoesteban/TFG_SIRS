using SIRS.Domain.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Application.ViewModels;

public class SalaViewModel
{
    [Key]
    public int Id { get; set; } // No autoincremental

    public string NombreCorto { get; set; }

    public string Descripcion { get; set; }

    public int Capacidad { get; set; }

    public int EstadoSalaId { get; set; }

    public int EdificioId { get; set; } 
    public ICollection<ReservaViewModel> Reservas { get; set; } = new List<ReservaViewModel>();

}
