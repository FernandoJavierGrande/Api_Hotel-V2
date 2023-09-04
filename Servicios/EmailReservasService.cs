using Api_Hotel_V2.DTOs;
using Api_Hotel_V2.DTOs.ReservasDTOs;

namespace Api_Hotel_V2.Servicios
{
    public class EmailReservasService :  IEmailReservasService
    {
        private readonly IEmailService _emailService;

        //private readonly IConfiguration configuration;

        public EmailReservasService(IEmailService emailService)
        {
            this._emailService = emailService;
            //this.configuration = configuration;
        }

        public void SendEmailNuevaReserva(ReservaDTOMail reservaDTO)
        {
            string dias = string.Empty;
            foreach (var dia in reservaDTO.Dias)
            {
                dias += $" -{dia.Day}/{dia.Month}/{dia.Year}";
            }

            EmailDTO mail = new EmailDTO();

            mail.Para = reservaDTO.Afiliado.Email;
            mail.Asunto = $"Reserva de {reservaDTO.Afiliado.Apellido} {reservaDTO.Afiliado.Nombre}";
            mail.Contenido = $"Se resgistró una reserva para el afiliado n° {reservaDTO.Afiliado.NumAfiliado}\n" +
                $"Para el/los dia/s: {dias}.\n" +
                $"La/las habitacion/es: {String.Join(", ", reservaDTO.Habitaciones)}\n" +
                $"Observaciones: {reservaDTO.EstadoPago}. -{reservaDTO.Obs}\n" +
                $"Cualquier modificacion y/o consulta comunicarse con la administracion.";


            _emailService.SendEmail(mail);
        }

    }
}
