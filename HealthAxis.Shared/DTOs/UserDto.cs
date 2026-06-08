using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs
{
    public class UserDto
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; }

        public int? ReferenceId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}