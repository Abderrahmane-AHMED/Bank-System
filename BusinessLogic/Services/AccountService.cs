using Interfaces.Services;
using Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;


namespace BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccount _accountRepository;
        private readonly IEmailSender _emailSender;

        public AccountService(IAccount accountRepository, IEmailSender emailSender)
        {
            _accountRepository = accountRepository;
            _emailSender = emailSender;
        }

        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _accountRepository.FindByEmailAsync(email);
            if (user == null) return;

            var token = await _accountRepository.GeneratePasswordResetTokenAsync(user);

            var callbackUrl = $"https://yourdomain.com/Account/ResetPassword?email={email}&token={Uri.EscapeDataString(token)}";

            await _emailSender.SendEmailAsync(email, "Reset Password",
                $"Click here to reset your password: <a href='{callbackUrl}'>Link</a>");
        }

        public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _accountRepository.FindByEmailAsync(email);
            if (user == null) return false;

            var result = await _accountRepository.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }
    }
}
