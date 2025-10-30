using System.ComponentModel.DataAnnotations;

namespace BankSystem.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "Please Enter First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please Enter Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public bool IsActive { get; set; }
    }
}
