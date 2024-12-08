using System.Net.Mail;
using System.Net;

namespace RegisterLogin.Services
{
    public class EmailService
    {
        private readonly string _email;
        private readonly string _password;

        public EmailService(string email, string password)
        {
            _email = email;
            _password = password;
        }

        public void SendEmail(string recipientEmail, string subject, string body)
        {
            using (var smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential(_email, _password);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_email),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(recipientEmail);

                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (SmtpException ex)
                {
                    // Handle the exception (log it, etc.)
                    throw; // or log the exception
                }
            }
        }
    }
}
