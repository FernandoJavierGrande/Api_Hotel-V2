using Api_Hotel_V2.DTOs.ReservacionesDTOs;
using Api_Hotel_V2.Entidades;

namespace Api_Hotel_V2.DTOs.ReservasDTOs
{
    public class ReservaDTOconReservaciones : ReservaDTO
    {
        public List<ReservacionDTO> ReservacionesDTO { get; set; }
    }
}
