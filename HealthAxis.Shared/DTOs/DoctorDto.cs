using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Doctor name is required.")]
        [Display(Name = "Full Name")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*(?: [a-zA-Z]+)*$", ErrorMessage = "Name must start with a capital letter and contain only letters and spaces.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Specialisation is required.")]
        public int Specialisation { get; set; }

        public string SpecialisationName { get; set; }

        [Required(ErrorMessage = "Years of experience is required.")]
        [Display(Name ="Years of Experience")]
        [Range(0, 70, ErrorMessage = "Years of experience must be between 0 and 70.")]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = "Consultation fee is required.")]
        [Display(Name = "Consultation Fees")]
        [Range(0, 10000, ErrorMessage = "Fee must be between 0 and 10,000.")]
        public decimal ConsultationFee { get; set; }

        public int UpcomingAppointmentCount{get; set;}
        public bool IsActive { get; set; }
    }
}