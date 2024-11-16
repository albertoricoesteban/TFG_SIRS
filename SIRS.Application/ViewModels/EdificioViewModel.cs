using SIRS.Domain.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Application.ViewModels;

public class EdificioViewModel
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "The Description is Required")]
    [MaxLength(255, ErrorMessage = "Description cannot exceed 255 characters")]
    [DisplayName("Description")]
    public string Descripcion { get; set; }

    [Required(ErrorMessage = "The Address is Required")]
    [MaxLength(255, ErrorMessage = "Address cannot exceed 255 characters")]
    [DisplayName("Address")]
    public string Direccion { get; set; }

    [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
    [DisplayName("Latitude")]
    public decimal? Latitud { get; set; }

    [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
    [DisplayName("Longitude")]
    public decimal? Longitud { get; set; }

    public ICollection<Sala> Salas { get; set; } = new List<Sala>(); // Inicializamos como una lista vacía si no hay salas

}
