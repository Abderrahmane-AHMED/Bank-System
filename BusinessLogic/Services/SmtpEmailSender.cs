
using System;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;


namespace BusinessLogic.Services
{
    public class SimpleEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            System.Console.WriteLine($"Send email to {email}");
            System.Console.WriteLine($"Subject: {subject}");
            System.Console.WriteLine($"Message: {htmlMessage}");
            return Task.CompletedTask;
        }
    }
}
