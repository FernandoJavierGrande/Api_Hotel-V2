using Api_Hotel_V2.DTOs.ReservasDTOs;

namespace Api_Hotel_V2.Servicios
{
    public interface IEmailReservasService
    {
        void SendEmailNuevaReserva(ReservaDTOMail reservaDTO);
        void SendEmailModReserva(ReservaDTOMail reservaDTOMail);
    }
}
