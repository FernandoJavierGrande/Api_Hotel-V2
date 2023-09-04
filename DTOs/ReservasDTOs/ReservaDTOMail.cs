using Api_Hotel_V2.DTOs.ReservacionesDTOs;
using Api_Hotel_V2.Entidades;

namespace Api_Hotel_V2.DTOs.ReservasDTOs
{
    public class ReservaDTOMail: IReservaConReservacionesDTO
    {
        public int Id { get; set; }
        public int NumAfiliado { get; set; }
        public string EstadoPago { get; set; }
        public string Obs { get; set; }
        public string Acompaniantes { get; set; }
        public Afiliado Afiliado { get; set; }
        public List<ReservacionDTO> ReservacionesDTO { get; set; }
        public List<DateTime> Dias { get; set; }
        public List<int> Habitaciones { get; set; }

    }
}
