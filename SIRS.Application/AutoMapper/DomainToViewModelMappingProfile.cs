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
        CreateMap<Usuario, UsuarioPerfilViewModel>()
         .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
         .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
         .ForMember(dest => dest.Apellido1, opt => opt.MapFrom(src => src.Apellido1))
         .ForMember(dest => dest.Apellido2, opt => opt.MapFrom(src => src.Apellido2))
         .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
         .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(src => src.FechaRegistro)).ReverseMap();
    }
}
