using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [Range(0, 2, ErrorMessage = "Gender must be 0, 1, or 2.")]
        public int Gender { get; set; }

        public string GenderName { get; set; }

        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email Address is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        public string Email { get; set; }

        [RegularExpression(@"^INS\d{4}$", ErrorMessage = "Insurance ID must be in the format 'INS' followed by 4 digits.")]
        public string InsuranceID { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}