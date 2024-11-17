using SIRS.Domain.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Application.ViewModels;

public class EdificioViewModel
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [MaxLength(255, ErrorMessage = "El campo descripción no puede acceder los 255 caracteres")]
    [DisplayName("Descripcion")]
    public string Descripcion { get; set; }

    [Required(ErrorMessage = "La dirección es obligatoria")]
    [MaxLength(255, ErrorMessage = "El campo dirección no puede acceder los 255 caracteres")]
    [DisplayName("Direccion")]
    public string Direccion { get; set; }

    [Range(-90, 90, ErrorMessage = "La Latitud debe estar entre -90 y 90")]
    [DisplayName("Latitud")]
    public decimal? Latitud { get; set; }

    [Range(-180, 180, ErrorMessage = "La Longitud debe estar entre -180 y 180")]
    [DisplayName("Longitud")]
    public decimal? Longitud { get; set; }

    public ICollection<SalaViewModel> Salas { get; set; } = new List<SalaViewModel>(); // Inicializamos como una lista vacía si no hay salas

}
