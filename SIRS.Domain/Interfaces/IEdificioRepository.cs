using SIRS.Domain.Models;

namespace SIRS.Domain.Interfaces;

public interface IEdificioRepository : IRepository<Edificio>
{
    Edificio GetById(int id);
    IEnumerable<Edificio> GetAll();
    void Add(Edificio edificio);
    void Update(Edificio edificio);
    void Delete(int id);
    IEnumerable<Edificio> SearchByName(string name);

    IEnumerable<Edificio> GetEdificiosByFilter(string descripcion, string direccion);
}
