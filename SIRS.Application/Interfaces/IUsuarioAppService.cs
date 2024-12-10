using SIRS.Application.ViewModels;
using SIRS.Domain.Models;
using System.Collections.Generic;

namespace SIRS.Application.Interfaces
{
    public interface IUsuarioAppService
    {
        UsuarioViewModel GetById(int id);
        void Add(UsuarioViewModel usuario);
        void Update(UsuarioViewModel usuario);
        
        void Delete(int id);
        IEnumerable<UsuarioViewModel> SearchByName(string name);
        IEnumerable<UsuarioViewModel> GetByRol(string rolNombre);
        IEnumerable<UsuarioViewModel> GetAllUsuarios();
        
        IEnumerable<UsuarioViewModel> SearchByFilters(string? username = null, string? nombre = null, string? apellido1 = null, string? apellido2 = null, string? email = null, DateTime? fechaRegistro = null, int? rolId = null);

        bool UserExistsByUsername(string username);

        bool UserExistsByEmail(string email);
        void UpdateUsuarioPerfil(int id, UsuarioPerfilViewModel model);
        IEnumerable<UsuarioViewModel> GetUsuariosByFilter(string username, string nombre, string apellido1, string apellido2);
    }
}
