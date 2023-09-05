using Api_Hotel_V2.DTOs;
using Api_Hotel_V2.DTOs.ReservasDTOs;

namespace Api_Hotel_V2.Servicios
{
    public class EmailReservasService : IEmailReservasService
    {
        private readonly IEmailService _emailService;

        public EmailReservasService(IEmailService emailService)
        {
            this._emailService = emailService;
        }

        public void SendEmailModReserva(ReservaDTOMail reservaDTO)
        {
            string dias = CantidadDias(reservaDTO);
            EmailDTO mail = new EmailDTO();

            mail.Para = reservaDTO.Afiliado.Email;
            mail.Asunto = $"Modificacion en la fecha de reserva de {reservaDTO.Afiliado.Apellido} {reservaDTO.Afiliado.Nombre}";
            mail.Contenido = $"Se resgistró la modificacion de la reserva n°:{reservaDTO.Id} del afiliado n° {reservaDTO.Afiliado.NumAfiliado}\n" +
                $"Fechas ya modificadas: {dias}.\n" +
                $"Habitacion: {String.Join(", ", reservaDTO.Habitaciones)}\n" +
                $"Observaciones: {reservaDTO.EstadoPago}. -{reservaDTO.Obs}\n" +
                $"Cualquier modificacion y/o consulta comunicarse con la administracion.";

            _emailService.SendEmail(mail);
        }

        public void SendEmailNuevaReserva(ReservaDTOMail reservaDTO)
        {
            string dias = CantidadDias(reservaDTO);

            EmailDTO mail = new EmailDTO();

            mail.Para = reservaDTO.Afiliado.Email;
            mail.Asunto = $"Reserva de {reservaDTO.Afiliado.Apellido} {reservaDTO.Afiliado.Nombre}";
            mail.Contenido = $"Se resgistró una nueva reserva n°:{reservaDTO.Id} para el afiliado n° {reservaDTO.Afiliado.NumAfiliado}\n" +
                $"Para el/los dia/s: {dias}.\n" +
                $"La/las habitacion/es: {String.Join(", ", reservaDTO.Habitaciones)}\n" +
                $"Observaciones: {reservaDTO.EstadoPago}. -{reservaDTO.Obs}\n" +
                $"Cualquier modificacion y/o consulta comunicarse con la administracion.";


            _emailService.SendEmail(mail);
        }

        private string CantidadDias( ReservaDTOMail reservaDTO)
        {
            string dias = string.Empty;
            foreach (var dia in reservaDTO.ReservacionesDTO)
            {
                dias += $" -{dia.Fecha.Day}/{dia.Fecha.Month}/{dia.Fecha.Year}";
            }
            return dias;
        }
    }
}
