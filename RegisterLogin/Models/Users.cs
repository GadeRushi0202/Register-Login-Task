using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegisterLogin.Models
{
    [Table("Users")]
    public class Users
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required]
        public DateTime DoB {  get; set; }

        [Required]
        public string? Gender { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\(\+91\)[0-9]{10}$", ErrorMessage = "Phone number must be in the format (+91)1234567890")]
        public string? PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{6,}$",
            ErrorMessage = "Password must be at least 6 characters long, contain at least 1 letter, 1 number, and 1 special character.")]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string? ConfirmPassword { get; set; }

        public string? PasswordResetToken { get; set; } // Stores the reset token
        public DateTime? ResetTokenExpiry { get; set; } // Expiry time for the token
    }
}
