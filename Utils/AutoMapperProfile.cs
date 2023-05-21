using Api_Hotel_V2.DTOs.AuthDTOs;
using Api_Hotel_V2.DTOs.EntidadesDTOs;
using Api_Hotel_V2.DTOs.HabitacionDTOs;
using Api_Hotel_V2.Entidades;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Api_Hotel_V2.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IdentityUser, UsuarioDTO>();
            //Afiliado
            CreateMap<Afiliado, AfiliadoDTO>().ReverseMap();
            CreateMap<AfiliadoCreacionDTO, Afiliado>();

            //Habitacion
            CreateMap<Habitacion, HabitacionDTO>().ReverseMap();
            CreateMap<HabitacionCreacionDTO, Habitacion>();
        }
    }
}
