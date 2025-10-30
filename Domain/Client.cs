using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class TbClient
    {
        [Key]
        public int ClientId { get; init; }

        [Required]
        [Range(1000, 9999, ErrorMessage = "PinCode must be exactly 4 digits.")]
        public int PinCode { get; set; }

        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "PhoneNumber must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Balance must be a non-negative number.")]
        public decimal Balance { get; set; }

        [Required(ErrorMessage = "Account Number is required.")]

        public string AccountNumber { get; init; }

        [Required(ErrorMessage = "First Name is required.")]
        [StringLength(50, ErrorMessage = "First Name must be less than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        [StringLength(50, ErrorMessage = "Last Name must be less than 50 characters.")]
        public string LastName { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }


    }
}
