using SIRS.Domain.Models;

namespace SIRS.Domain.Interfaces;

public interface ISalaRepository : IRepository<Sala>
{
    Sala GetById(int id);
    IEnumerable<Sala> GetAllSalas();
    void Add(Sala sala);
    void Update(Sala sala);
    void Delete(int id);
    IEnumerable<Sala> SearchByDescripcion(string descripcion);
    IEnumerable<Sala> GetByEstado(string estado);
    IEnumerable<Sala> GetSalasByFilter(string nombreCorto, int capacidad, int edificioId);
    IEnumerable<Sala> GetByEdificioId(int EdificioId);

    IEnumerable<Sala> GetSalasByFilterWithEdificioNombre();
    Sala GetByIdWithEdificio(int id);
}
