using AutoMapper;
using SIRS.Application.ViewModels;
using SIRS.Data.Repository;
using SIRS.Domain.Interfaces;
using SIRS.Domain.Models;
using System;
using System.Collections.Generic;

namespace SIRS.Application.Interfaces
{
    public class RolAppService : IRolAppService
    {
        private readonly IMapper _mapper;
        private readonly IRolRepository _rolRepository;

        public RolAppService(
            IMapper mapper,
            IRolRepository rolRepository)
        {
            _mapper = mapper;
            _rolRepository = rolRepository;
        }

        public RoleViewModel GetById(int id)
        {
            var roles = _rolRepository.GetById(id);
            return _mapper.Map<RoleViewModel>(roles);
        }
        public IEnumerable<RoleViewModel> GetAll()
        {
            var roles = _rolRepository.GetAll();
            return _mapper.Map<IEnumerable<RoleViewModel>>(roles);

        }
    }
}
