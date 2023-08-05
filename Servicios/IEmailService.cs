using Api_Hotel_V2.DTOs;

namespace Api_Hotel_V2.Servicios
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO email);
    }
}
