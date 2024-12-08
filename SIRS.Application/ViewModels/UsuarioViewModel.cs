using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIRS.Application.ViewModels;

public class UsuarioViewModel
{
    [Key]
    public int Id { get; set; } 

    [Required(ErrorMessage = "El DNI/NIE es obligatorio.")]
    [RegularExpression(@"^[XYZ]?\d{5,8}[A-Z]$", ErrorMessage = "El DNI/NIE no es válido.")]
    public string Username { get; set; }


    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El primer apellido es obligatorio.")]
    public string Apellido1 { get; set; }

    public string Apellido2 { get; set; } // Segundo apellido opcional

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "Por favor, introduce un email válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    
    public string Password { get; set; }

    [Required(ErrorMessage = "El rol es obligatorio.")]
    public int RolId { get; set; }
    public DateTime FechaRegistro { get; set; }
    // public IEnumerable<SelectListItem> Roles { get; set; } // Lista de roles para el dropdown

}
