using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Specialisation is required.")]
        public string Specialisation { get; set; }

        [Required(ErrorMessage = "Years of experience is required.")]
        [Range(0, 60)]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = "Consultation fee is required.")]
        [Range(typeof(decimal), "1", "999999")]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}