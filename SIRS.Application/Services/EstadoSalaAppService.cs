using AutoMapper;
using SIRS.Application.Interfaces;
using SIRS.Application.ViewModels;
using SIRS.Domain.Interfaces;
using SIRS.Domain.Models;
using System.Collections.Generic;

namespace SIRS.Application.Services
{
    public class EstadoSalaAppService : IEstadoSalaAppService
    {
        private readonly IMapper _mapper;
        private readonly IEstadoSalaRepository _estadoSalaRepository;

        public EstadoSalaAppService(
            IMapper mapper,
            IEstadoSalaRepository estadoSalaRepository)
        {
            _mapper = mapper;
            _estadoSalaRepository = estadoSalaRepository;
        }

        public EstadoSalaViewModel GetById(int id)
        {
            var estadoSala = _estadoSalaRepository.GetById(id);
            return _mapper.Map<EstadoSalaViewModel>(estadoSala);
        }

        public void Add(EstadoSalaViewModel estadoSalaViewModel)
        {
            var estadoSala = _mapper.Map<EstadoSala>(estadoSalaViewModel);
            _estadoSalaRepository.Add(estadoSala);
        }

        public void Update(EstadoSalaViewModel estadoSalaViewModel)
        {
            var estadoSala = _mapper.Map<EstadoSala>(estadoSalaViewModel);
            _estadoSalaRepository.Update(estadoSala);
        }

        public void Delete(int id)
        {
            _estadoSalaRepository.Delete(id);
        }

        public IEnumerable<EstadoSalaViewModel> GetAllEstados()
        {
            var estadosSala = _estadoSalaRepository.GetAllEstados();
            return _mapper.Map<IEnumerable<EstadoSalaViewModel>>(estadosSala);
        }
    }
}
