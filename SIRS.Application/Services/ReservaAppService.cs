﻿using AutoMapper;
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
        private readonly ISalaRepository _salaRepository;
        public ReservaAppService(
            IMapper mapper,
            IReservaRepository reservaRepository, ISalaRepository salaRepository)
        {
            _mapper = mapper;
            _reservaRepository = reservaRepository;
            _salaRepository = salaRepository;
        }

        public ReservaViewModel GetById(int id)
        {
            var reserva = _reservaRepository.GetById(id);
            var elementoARetornar = _mapper.Map<ReservaViewModel>(reserva);
            var edificio = _salaRepository.GetByIdWithEdificio(elementoARetornar.SalaId);
            elementoARetornar.EdificioId = edificio.EdificioId;
            return elementoARetornar;
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

        public IEnumerable<ReservaViewModel> GetReservasByFilters(int salaId, DateTime? fechaReserva, TimeSpan? horaInicio, int? usuarioId = null)
        {
            var reservas = _reservaRepository.GetReservasByFilters(salaId, fechaReserva, horaInicio,usuarioId);


            return _mapper.Map<IEnumerable<ReservaViewModel>>(reservas);
        }

        public IEnumerable<ReservaViewModel> ObtenerReservasCalendario(DateTime fechaInicio, DateTime fechaFin,  int? salaId = null)

        {
            var reservas = _reservaRepository.ObtenerReservasCalendario(fechaInicio,fechaFin, null,salaId);


            return _mapper.Map<IEnumerable<ReservaViewModel>>(reservas);
        }

        public void CancelarReserva(int id, int usuarioGestionId)
        {
            _reservaRepository.CancelarReserva(id, usuarioGestionId);
        }
        public void ReactivarReserva(int id, int usuarioGestionId)
        {
            _reservaRepository.ReactivarReserva(id, usuarioGestionId);

        }
        public void AprobarReserva(int id, int usuarioGestionId)
        {
            _reservaRepository.AprobarReserva(id, usuarioGestionId);
        }
    }
}

