using Domain;
using Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class AccountRepository : IAccount
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApplicationUser?> FindByEmailAsync(string email)
            => await _userManager.FindByEmailAsync(email);

        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
            => await _userManager.GeneratePasswordResetTokenAsync(user);

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
            => await _userManager.ResetPasswordAsync(user, token, newPassword);
    }
}
