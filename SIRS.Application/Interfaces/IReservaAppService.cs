using SIRS.Application.ViewModels;
using SIRS.Domain.Models;
using System;
using System.Collections.Generic;

namespace SIRS.Application.Interfaces
{
    public interface IReservaAppService
    {
        ReservaViewModel GetById(int id);
        void Add(ReservaViewModel reserva);
        void Update(ReservaViewModel reserva);
        void Delete(int id);
        IEnumerable<ReservaViewModel> GetBySala(int salaId);
        IEnumerable<ReservaViewModel> GetByUsuario(int usuarioId);
        IEnumerable<ReservaViewModel> GetByFecha(DateTime fecha);
        IEnumerable<ReservaViewModel> GetAll();
        IEnumerable<ReservaViewModel> GetReservasByFilters(int salaId, DateTime? fechaReserva, TimeSpan? horaInicio);

        IEnumerable<ReservaViewModel> ObtenerReservasCalendario(DateTime fechaInicio, DateTime fechaFin);

    }
}
