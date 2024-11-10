using SIRS.Domain.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Application.ViewModels;

public class SalaViewModel
{
    [Key]
    public int Id { get; set; } // No autoincremental

    [Required(ErrorMessage = "The Role Name is Required")]
    [MaxLength(100, ErrorMessage = "Role name cannot exceed 100 characters")]
    [DisplayName("Role Name")]
    public string Nombre { get; set; }

    public ICollection<UsuarioViewModel> Usuarios { get; set; }

}
