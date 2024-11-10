using AutoMapper;
using SIRS.Application.Interfaces;
using SIRS.Application.ViewModels;
using SIRS.Domain.Interfaces;
using SIRS.Domain.Models;
using System.Collections.Generic;

namespace SIRS.Application.Services
{
    public class SalaAppService : ISalaAppService
    {
        private readonly IMapper _mapper;
        private readonly ISalaRepository _salaRepository;

        public SalaAppService(
            IMapper mapper,
            ISalaRepository salaRepository)
        {
            _mapper = mapper;
            _salaRepository = salaRepository;
        }

        public SalaViewModel GetById(int id)
        {
            var sala = _salaRepository.GetById(id);
            return _mapper.Map<SalaViewModel>(sala);
        }

        public void Add(SalaViewModel salaViewModel)
        {
            var sala = _mapper.Map<Sala>(salaViewModel);
            _salaRepository.Add(sala);
        }

        public void Update(SalaViewModel salaViewModel)
        {
            var sala = _mapper.Map<Sala>(salaViewModel);
            _salaRepository.Update(sala);
        }

        public void Delete(int id)
        {
            _salaRepository.Delete(id);
        }

        public IEnumerable<SalaViewModel> SearchByDescripcion(string descripcion)
        {
            var salas = _salaRepository.SearchByDescripcion(descripcion);
            return _mapper.Map<IEnumerable<SalaViewModel>>(salas);
        }

        public IEnumerable<SalaViewModel> GetByEstado(string estado)
        {
            var salas = _salaRepository.GetByEstado(estado);
            return _mapper.Map<IEnumerable<SalaViewModel>>(salas);
        }

        public IEnumerable<SalaViewModel> GetAllSalas()
        {
            var salas = _salaRepository.GetAllSalas();
            return _mapper.Map<IEnumerable<SalaViewModel>>(salas);
        }
    }
}
