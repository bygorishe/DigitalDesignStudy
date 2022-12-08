using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Common.Services
{
    public static class EmailService
    {
        public static async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация", "fredrick77@ethereal.email"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                //await client.ConnectAsync("smtp.yandex.ru", 465, true); /////////////////////
                await client.AuthenticateAsync("fredrick77@ethereal.email", "yFZa41yShVcG2d5AJY");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
