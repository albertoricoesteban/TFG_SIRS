using AutoMapper;
using SIRS.Application.Interfaces;
using SIRS.Application.ViewModels;
using SIRS.Data.Repository;
using SIRS.Domain.Interfaces;
using SIRS.Domain.Models;
using System.Collections.Generic;

namespace SIRS.Application.Services
{
    public class AccountAppService : IAccountAppService
    {
        private readonly IMapper _mapper;
        private IUsuarioRepository _usuarioRepository;
        public AccountAppService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public AccountAppService(
            IMapper mapper,
            IUsuarioRepository usuarioRepository)
        {
            _mapper = mapper;
            _usuarioRepository = usuarioRepository;
        }

   
        public IEnumerable<UsuarioViewModel> SearchByFilters(string? username = null, string? nombre = null, string? apellido1 = null, string? apellido2 = null, string? email = null, DateTime? fechaRegistro = null, int? rolId = null)
        {
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(_usuarioRepository.SearchByFilter(username,nombre,apellido1,apellido2,email,fechaRegistro,rolId));
        }

        public IEnumerable<UsuarioViewModel> ExisteUserName(string username )
        {
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(_usuarioRepository.SearchByFilter(username));
        }

        public IEnumerable<UsuarioViewModel> ExisteEmail(string email)
        {
            return _mapper.Map<IEnumerable<UsuarioViewModel>>(_usuarioRepository.SearchByFilter(email));
        }
    }
}
