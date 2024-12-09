using SIRS.Domain.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Application.ViewModels;

public class ReservaViewModel
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El Nombre es obligatoria")]
    [MaxLength(200, ErrorMessage = "Nombre no puede ser mayor de 200 caracteres")]
    [DisplayName("Nombre")]
    public string Nombre { get; set; }

    [MaxLength(500, ErrorMessage = "Observations cannot exceed 500 characters")]
    [DisplayName("Observaciones")]
    public string? Observaciones { get; set; }

    [Required(ErrorMessage = "La FechaReserva es obligatoria")]
    [DisplayName("FechaReserva")]
    public DateTime? FechaReserva { get; set; }

    [Required(ErrorMessage = "La HoraInicio es obligatoria")]
    [DisplayName("HoraInicio")]
    public TimeSpan? HoraInicio { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El tiempo total tiene que ser un número positivo")]
    [DisplayName("TiempoTotal")]
    public int? TiempoTotal { get; set; }

    [Required]
    public int SalaId { get; set; }

    public int EdificioId { get; set; }
    public SalaViewModel? Sala { get; set; }
    public EdificioViewModel? Edificio{ get; set; }
    [Required]
    public int UsuarioId { get; set; }
    public UsuarioViewModel? Usuario { get; set; }

    public bool? Aprobada { get; set; }
}
