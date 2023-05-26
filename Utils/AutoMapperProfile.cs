using Api_Hotel_V2.DTOs.AuthDTOs;
using Api_Hotel_V2.DTOs.EntidadesDTOs;
using Api_Hotel_V2.DTOs.HabitacionDTOs;
using Api_Hotel_V2.DTOs.ReservacionesDTOs;
using Api_Hotel_V2.DTOs.ReservasDTOs;
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

            //reserva
            CreateMap<Reserva, ReservaDTO>().ReverseMap();
            CreateMap<ReservaCreacionDTO, Reserva>();
            CreateMap<Reserva, ReservaDTOconReservaciones>()
                .ForMember(r => r.ReservacionesDTO, opciones => opciones.MapFrom(MapReservacionesDTOReservas));

        }
        private List<ReservacionDTO> MapReservacionesDTOReservas(Reserva reserva, ReservaDTOconReservaciones reservaDTOconReservaciones)
        {
            var resultado = new List<ReservacionDTO>();

            foreach (var reservacion in reserva.Reservaciones)
            {
                resultado.Add(new ReservacionDTO()
                {
                    HabitacionNum = reservacion.Habitacion.NumHab,
                    Fecha = reservacion.Fecha,
                });
            }
            return resultado;
        }
    }
}
