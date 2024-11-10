using SIRS.Domain.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Application.ViewModels;

public class ReservaViewModel
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "The Name is Required")]
    [MaxLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    [DisplayName("Name")]
    public string Nombre { get; set; }

    [MaxLength(500, ErrorMessage = "Observations cannot exceed 500 characters")]
    [DisplayName("Observations")]
    public string Observaciones { get; set; }

    [Required(ErrorMessage = "The Reservation Date is Required")]
    [DisplayName("Reservation Date")]
    public DateTime? FechaReserva { get; set; }

    [Required(ErrorMessage = "The Start Time is Required")]
    [DisplayName("Start Time")]
    public TimeSpan? HoraInicio { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Total Time must be a positive number")]
    [DisplayName("Total Time (minutes)")]
    public int? TiempoTotal { get; set; }

    [Required]
    public int SalaId { get; set; }
    public SalaViewModel Sala { get; set; }

    [Required]
    public int UsuarioId { get; set; }
    public UsuarioViewModel Usuario { get; set; }
}
