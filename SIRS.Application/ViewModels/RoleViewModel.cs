using SIRS.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRS.Application.ViewModels
{
    public class RoleViewModel
    {
      
        public int Id { get; set; } 

        public string Nombre { get; set; }
        public ICollection<UsuarioViewModel>? Usuarios { get; set; } // Propiedad opcional


    }
}
