using SIRS.Domain.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Application.ViewModels;

public class EstadoSalaViewModel
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "The Description is Required")]
    [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
    [DisplayName("Description")]
    public string Descripcion { get; set; }

    public ICollection<Sala> Salas { get; set; }
}
