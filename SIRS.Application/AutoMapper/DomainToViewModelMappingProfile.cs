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
        CreateMap<Rol, RoleViewModel>()
    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)) // Mapear Id
    .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre)) // Mapear Nombre
    .ForMember(dest => dest.Usuarios, opt => opt.Ignore()) // Ignorar la propiedad Usuarios si está presente
    .ReverseMap();



    }
}
