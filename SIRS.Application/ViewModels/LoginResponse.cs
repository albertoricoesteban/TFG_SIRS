using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRS.Application.ViewModels
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Username { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }

        public string Email { get; set; }
        public int RolId { get; set; }
    }
}
