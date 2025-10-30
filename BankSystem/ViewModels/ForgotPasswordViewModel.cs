using System.ComponentModel.DataAnnotations;

namespace BankSystem.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
