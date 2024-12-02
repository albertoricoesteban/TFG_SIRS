using AutoMapper;
using SIRS.Application.Interfaces;
using SIRS.Application.ViewModels;
using SIRS.Domain.Interfaces;
using SIRS.Domain.Models;
using System;
using System.Collections.Generic;

namespace SIRS.Application.Services
{
    public class ReservaAppService : IReservaAppService
    {
        private readonly IMapper _mapper;
        private readonly IReservaRepository _reservaRepository;

        public ReservaAppService(
            IMapper mapper,
            IReservaRepository reservaRepository)
        {
            _mapper = mapper;
            _reservaRepository = reservaRepository;
        }

        public ReservaViewModel GetById(int id)
        {
            var reserva = _reservaRepository.GetById(id);
            return _mapper.Map<ReservaViewModel>(reserva);
        }

        public void Add(ReservaViewModel reservaViewModel)
        {
            var reserva = _mapper.Map<Reserva>(reservaViewModel);
            _reservaRepository.Add(reserva);
        }

        public void Update(ReservaViewModel reservaViewModel)
        {
            var reserva = _mapper.Map<Reserva>(reservaViewModel);
            _reservaRepository.Update(reserva);
        }

        public void Delete(int id)
        {
            _reservaRepository.Delete(id);
        }

        public IEnumerable<ReservaViewModel> GetBySala(int salaId)
        {
            var reservas = _reservaRepository.GetBySala(salaId);
            return _mapper.Map<IEnumerable<ReservaViewModel>>(reservas);
        }

        public IEnumerable<ReservaViewModel> GetByUsuario(int usuarioId)
        {
            var reservas = _reservaRepository.GetByUsuario(usuarioId);
            return _mapper.Map<IEnumerable<ReservaViewModel>>(reservas);
        }

        public IEnumerable<ReservaViewModel> GetByFecha(DateTime fecha)
        {
            var reservas = _reservaRepository.GetByFecha(fecha);
            return _mapper.Map<IEnumerable<ReservaViewModel>>(reservas);
        }

        public IEnumerable<ReservaViewModel> GetAll()
        {
            var reservas = _reservaRepository.GetAll();


            return _mapper.Map<IEnumerable<ReservaViewModel>>(reservas);
        }

        public IEnumerable<ReservaViewModel> GetReservasByFilters(int salaId, DateTime? fechaReserva, TimeSpan? horaInicio)
        {
            var reservas = _reservaRepository.GetReservasByFilters(salaId, fechaReserva, horaInicio);


            return _mapper.Map<IEnumerable<ReservaViewModel>>(reservas);
        }
    }
}

