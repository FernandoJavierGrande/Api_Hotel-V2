using Api_Hotel_V2.DTOs.ReservacionesDTOs;

namespace Api_Hotel_V2.DTOs.ReservasDTOs
{
    public interface IReservaConReservacionesDTO
    {
        public List<ReservacionDTO> ReservacionesDTO { get; set; }
        public int NumAfiliado { get; set; }
    }
}
