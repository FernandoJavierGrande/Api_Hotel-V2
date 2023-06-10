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

            //reserva
            CreateMap<Reserva, ReservaDTO>().ReverseMap();
            CreateMap<Reserva, ReservaDTOconReservaciones>()
                .ForMember(a => a.NumAfiliado, opt => { opt.MapFrom(r => r.Afiliado.NumAfiliado); })
                .ForMember(r => r.ReservacionesDTO, opciones => opciones.MapFrom(MapReservacionesDTOReservas)).ReverseMap();
            CreateMap<ReservaCreacionDTO, Reserva>()
                .ForMember(x => x.Reservaciones, opciones => opciones.MapFrom(MapReservaCreacionDTOReservas));
            CreateMap<Reserva, PatchReservaEstadoDTO>().ReverseMap();

            //reservacion
            CreateMap<Reservacion, ReservacionDTO>().ReverseMap();
            CreateMap<Reservacion, ReservacionCreacionDTO>().ReverseMap();

        }

        private List<ReservacionDTO> MapReservacionesDTOReservas(Reserva reserva, ReservaDTOconReservaciones reservaDTOconReservaciones)
        {
            var resultado = new List<ReservacionDTO>();

            foreach (var reservacion in reserva.Reservaciones)
            {
                resultado.Add(new ReservacionDTO()
                {
                    HabitacionId = reservacion.Habitacion.Id,
                    Fecha = reservacion.Fecha,
                });
            }
            return resultado;
        }
        private List<Reservacion> MapReservaCreacionDTOReservas(ReservaCreacionDTO reservaCreacionDTO, Reserva reserva)
        {
            var reservaciones = new List<Reservacion>();

            foreach (var reservacionDto in reservaCreacionDTO.reservaciones)
            {
                reservaciones.Add(new Reservacion()
                {
                    HabitacionId = reservacionDto.HabitacionId,
                    Fecha= reservacionDto.Fecha,
                });

            }
            return reservaciones;
        }
    }
}
