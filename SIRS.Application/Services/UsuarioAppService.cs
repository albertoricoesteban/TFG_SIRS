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

        public void Delete(int id)
        {
            _usuarioRepository.Delete(id);
        }

        public IEnumerable<UsuarioViewModel> SearchByName(string name)
        {
            var usuarios = _usuarioRepository.SearchByName(name);
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(usuarios);
        }
        public IEnumerable<UsuarioViewModel> SearchByFilters(string? username = null, string? nombre = null, string? apellido1 = null, string? apellido2 = null, string? email = null, DateTime? fechaRegistro = null, int? rolId = null)
        {
            // Llamamos al repositorio con los filtros proporcionados
            var usuarios = _usuarioRepository.SearchByFilter(username, nombre, apellido1, apellido2, email, fechaRegistro, rolId);

            // Mapeamos los usuarios a UsuarioViewModel
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(usuarios);
        }
        public IEnumerable<UsuarioViewModel> GetByRol(string rolNombre)
        {
            var usuarios = _usuarioRepository.GetByRol(rolNombre);
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(usuarios);
        }
        public bool UserExistsByUsername(string username)
        {
            var usuarios = _usuarioRepository.UserExistsByUsername(username);
            return usuarios;
        }
        public bool UserExistsByEmail(string email)
        {
            var usuarios = _usuarioRepository.UserExistsByEmail(email);
            return usuarios;
        }
        public void UpdateUsuarioPerfil(int id, UsuarioPerfilViewModel model, bool edicionAdmin)
        {
            var usuario = _mapper.Map<Usuario>(model);
            _usuarioRepository.UpdateUsuarioPerfil(id, usuario,edicionAdmin);
        }
        public IEnumerable<UsuarioViewModel> GetAllUsuarios()
        {
            var usuarios = _usuarioRepository.GetAllUsuarios();
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(usuarios);
        }
        public IEnumerable<UsuarioViewModel> GetUsuariosByFilter(string username, string nombre, string apellido1, string apellido2)
        {
            var usuarios = _usuarioRepository.SearchByFilter(username, nombre, apellido1, apellido2);
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(usuarios);
        }
    }
}
