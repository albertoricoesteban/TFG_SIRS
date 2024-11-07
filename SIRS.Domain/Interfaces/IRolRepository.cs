using SIRS.Domain.Models;

namespace SIRS.Domain.Interfaces;

public interface IRolRepository : IRepository<Rol>
{
    Rol GetById(int id);
    IEnumerable<Rol> GetAllRoles();
    void Add(Rol rol);
    void Update(Rol rol);
    void Delete(int id);
}
