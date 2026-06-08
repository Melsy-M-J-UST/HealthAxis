using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace HealthAxis.Shared.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public string UserId { get; set; }

        public int? ReferenceId { get; set; }
    }
}