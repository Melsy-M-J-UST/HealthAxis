using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Doctor name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Specialisation is required.")]
        public int Specialisation { get; set; }

        public string SpecialisationName { get; set; }

        [Required(ErrorMessage = "Years of experience is required.")]
        [Range(0, 70, ErrorMessage = "Years of experience must be between 0 and 70.")]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = "Consultation fee is required.")]
        [Range(0, 10000, ErrorMessage = "Fee must be between 0 and 10,000.")]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}