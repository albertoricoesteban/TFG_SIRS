using AutoMapper;

using SIRS.Application.ViewModels;
using SIRS.Domain.Models;

namespace SIRS.Application.AutoMapper;

public class DomainToViewModelMappingProfile : Profile
{
    public DomainToViewModelMappingProfile()
    {
        CreateMap<Edificio, EdificioViewModel>();
        CreateMap<Sala, SalaViewModel>();
        CreateMap<Reserva, ReservaViewModel>();
        CreateMap<Usuario, UsuarioViewModel>();
        CreateMap<EstadoSala, EstadoSalaViewModel>();
    }
}
