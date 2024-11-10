using SIRS.Domain.Models;

namespace SIRS.Domain.Interfaces;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Usuario GetById(int id);
    IEnumerable<Usuario> GetAllUsuarios();
    void Add(Usuario usuario);
    void Update(Usuario usuario);
    void Delete(int id);
    IEnumerable<Usuario> GetByRol(string rolNombre);
    IEnumerable<Usuario> SearchByName(string name);
}
