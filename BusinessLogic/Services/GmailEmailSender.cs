using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class GmailEmailSender : IEmailSender
    {
        private readonly string _fromEmail;
        private readonly string _password;

        public GmailEmailSender(string fromEmail, string password)
        {
            _fromEmail = fromEmail;
            _password = password;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Support", _fromEmail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;
            message.Body = new TextPart("html") { Text = htmlMessage };

            //  MailKit
            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_fromEmail, _password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
