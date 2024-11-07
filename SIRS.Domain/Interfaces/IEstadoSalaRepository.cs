using SIRS.Domain.Models;

namespace SIRS.Domain.Interfaces;

public interface IEstadoSalaRepository : IRepository<EstadoSala>
{
    EstadoSala GetById(int id);
    void Add(EstadoSala estadoSala);
    void Update(EstadoSala estadoSala);
    void Delete(int id);
    IEnumerable<EstadoSala> GetAllEstados();
}
