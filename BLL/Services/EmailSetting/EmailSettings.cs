using DAL.Models.Common;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BLL.Services.EmailSetting
{
    public class EmailSettings : IEmailSettings
    {
        public async Task SendEmailAsync(ResetEmail resetEmail)
        {
            // Note: In a real app, these values (host, port, credentials) 
            // should be in appsettings.json, not hardcoded.
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("moradlord974@gmail.com", "ipdyziftdnesjdpy")
            };

            // Using the built-in Async method for sending mail
            await client.SendMailAsync(new MailMessage(
                from: "moradlord974@gmail.com",
                to: resetEmail.To!,
                subject: resetEmail.Subject,
                body: resetEmail.Body
            ));
        }
    }
}