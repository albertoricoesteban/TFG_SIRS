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
    IEnumerable<Usuario> SearchByFilter(string? username = null, string? nombre = null, string? apellido1 = null, string? apellido2 = null, string? email = null, DateTime? fechaRegistro = null, int? rolId = null);
    bool UserExistsByUsername(string username);
    bool UserExistsByEmail(string email);
    void UpdateUsuarioPerfil(int id, Usuario model);
}
