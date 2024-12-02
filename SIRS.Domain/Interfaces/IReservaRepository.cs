using SIRS.Domain.Models;

namespace SIRS.Domain.Interfaces;

public interface IReservaRepository : IRepository<Reserva>
{
    Reserva GetById(int id);
    IEnumerable<Reserva> GetBySala(int salaId);
    IEnumerable<Reserva> GetByUsuario(int usuarioId);
    IEnumerable<Reserva> GetByFecha(DateTime fecha);
    void Add(Reserva reserva);
    void Update(Reserva reserva);
    void Delete(int id);
    IEnumerable<Reserva> GetReservasByFilters( int salaId, DateTime? fechaReserva, TimeSpan? horaInicio);

}
