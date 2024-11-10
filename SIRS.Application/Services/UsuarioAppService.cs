using AutoMapper;
using SIRS.Application.Interfaces;
using SIRS.Application.ViewModels;
using SIRS.Domain.Interfaces;
using SIRS.Domain.Models;
using System.Collections.Generic;

namespace SIRS.Application.Services
{
    public class UsuarioAppService : IUsuarioAppService
    {
        private readonly IMapper _mapper;
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioAppService(
            IMapper mapper,
            IUsuarioRepository usuarioRepository)
        {
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
        }

        public UsuarioViewModel GetById(int id)
        {
            var usuario = _usuarioRepository.GetById(id);
            return _mapper.Map<UsuarioViewModel>(usuario);
        }

        public void Add(UsuarioViewModel usuarioViewModel)
        {
            var usuario = _mapper.Map<Usuario>(usuarioViewModel);
            _usuarioRepository.Add(usuario);
        }

        public void Update(UsuarioViewModel usuarioViewModel)
        {
            var usuario = _mapper.Map<Usuario>(usuarioViewModel);
            _usuarioRepository.Update(usuario);
        }

        public void Delete(int id)
        {
            _usuarioRepository.Delete(id);
        }

        public IEnumerable<UsuarioViewModel> SearchByName(string name)
        {
            var usuarios = _usuarioRepository.SearchByName(name);
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(usuarios);
        }

        public IEnumerable<UsuarioViewModel> GetByRol(string rolNombre)
        {
            var usuarios = _usuarioRepository.GetByRol(rolNombre);
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(usuarios);
        }

        public IEnumerable<UsuarioViewModel> GetAllUsuarios()
        {
            var usuarios = _usuarioRepository.GetAllUsuarios();
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(usuarios);
        }
    }
}
