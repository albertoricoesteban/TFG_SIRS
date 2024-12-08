using System.ComponentModel.DataAnnotations;

namespace SIRS.Models
{
    public class UserViewModel
    {

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
        [StringLength(10, ErrorMessage = "La contraseña debe tener al menos {2} caracteres y máximo 10.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{10,}$",
            ErrorMessage = "La contraseña debe incluir una mayúscula, una minúscula, un número y un carácter especial.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public int RolId { get; set; }
    }
}
