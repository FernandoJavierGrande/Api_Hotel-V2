using Api_Hotel_V2.DTOs;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;

namespace Api_Hotel_V2.Servicios
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public EmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void SendEmail(EmailDTO emailDTO)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(configuration.GetSection("Email:userName").Value));
            email.To.Add(MailboxAddress.Parse(emailDTO.Para));
            email.Subject = emailDTO.Asunto;
            email.Body = new TextPart(TextFormat.Plain) { Text = emailDTO.Contenido };


            using var smtp = new SmtpClient();
            smtp.Connect(
                configuration.GetSection("Email:Host").Value,
                Convert.ToInt32(configuration.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
                );

            smtp.Authenticate(configuration.GetSection("Email:Username").Value, configuration.GetSection("Email:Password").Value);
            
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
