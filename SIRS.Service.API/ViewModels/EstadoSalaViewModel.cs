using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Service.API.ViewModels;

public class EstadoSalaViewModel
{
    public int Id { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}
