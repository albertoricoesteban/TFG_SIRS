using AutoMapper;
using SIRS.Application.Interfaces;
using SIRS.Application.ViewModels;
using SIRS.Domain.Interfaces;
using SIRS.Domain.Models;
using System.Collections.Generic;

namespace SIRS.Application.Services
{
    public class EdificioAppService : IEdificioAppService
    {
        private readonly IMapper _mapper;
        private readonly IEdificioRepository _edificioRepository;

        public EdificioAppService(
            IMapper mapper,
            IEdificioRepository edificioRepository)
        {
            _mapper = mapper;
            _edificioRepository = edificioRepository;
        }

        public EdificioViewModel GetById(int id)
        {
            return _mapper.Map<EdificioViewModel>(_edificioRepository.GetById(id));
        }

        public IEnumerable<EdificioViewModel> GetAll()
        {
            return _mapper.Map<IEnumerable<EdificioViewModel>>(_edificioRepository.GetAll());
        }

        public void Add(EdificioViewModel edificio)
        {
            var addEdificio = _mapper.Map<Edificio>(edificio);
            _edificioRepository.Add(addEdificio);
        }

        public void Update(EdificioViewModel edificio)
        {
            var updateEdificio = _mapper.Map<Edificio>(edificio);
            _edificioRepository.Update(updateEdificio);
        }

        public void Delete(int id)
        {
            _edificioRepository.Delete(id);
        }

        public IEnumerable<EdificioViewModel> SearchByName(string name)
        {
            return _mapper.Map<IEnumerable<EdificioViewModel>>(_edificioRepository.SearchByName(name));
        }
    }
}
