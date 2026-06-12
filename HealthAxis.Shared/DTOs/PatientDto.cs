using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs
{
    public class PatientDto : IValidatableObject
    {
        public int PatientId { get; set; }


        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*(?: [a-zA-Z]+)*$", ErrorMessage = "Name must start with a capital letter and contain only letters and spaces.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required.")]
        [Display(Name = "DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public int Gender { get; set; }

        public string GenderName { get; set; }


        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must contain 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        public string InsuranceID { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public int AppointmentCount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var minDob = new DateTime(1900, 1, 1);

            if (DateOfBirth < minDob)
            {
                yield return new ValidationResult(
                    "Date of Birth year must be 1900 or later.",
                    new[] { nameof(DateOfBirth) });
            }

            if (DateOfBirth >= DateTime.Today)
            {
                yield return new ValidationResult(
                    "Date of Birth must be before today.",
                    new[] { nameof(DateOfBirth) });
            }
        }
    }
}