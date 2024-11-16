using AutoMapper;

using SIRS.Application.ViewModels;
using SIRS.Domain.Models;

namespace SIRS.Application.AutoMapper;

public class DomainToViewModelMappingProfile : Profile
{
    public DomainToViewModelMappingProfile()
    {
        CreateMap<Edificio, EdificioViewModel>().ReverseMap();
        CreateMap<Sala, SalaViewModel>().ReverseMap();
        CreateMap<Reserva, ReservaViewModel>().ReverseMap();
        CreateMap<Usuario, UsuarioViewModel>().ReverseMap();
        CreateMap<EstadoSala, EstadoSalaViewModel>().ReverseMap();
    }
}
