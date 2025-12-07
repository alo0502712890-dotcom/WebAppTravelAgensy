using System.Net;
using System.Net.Mail;

namespace WebApp.Helpers
{
    public static class EmailSender
    {
        public static async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("alo0502712890@gmail.com", "tcytiaplzpxnnzmc"),
                EnableSsl = true,
            };

            var message = new MailMessage()
            {
                From = new MailAddress("alo0502712890@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(to);
            await smtp.SendMailAsync(message);
        }
    }
}
