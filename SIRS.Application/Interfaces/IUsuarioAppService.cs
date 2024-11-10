using SIRS.Application.ViewModels;
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
    }
}
