using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HealthAxis_Web.Models.Dtos
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialisation is required")]
        public string Specialisation { get; set; }

        [Required(ErrorMessage = "Years of Experience is required")]
        [Range(0, 50, ErrorMessage = "Enter valid experience")]
        public int YearsOfExperience { get; set; }

        [Required(ErrorMessage = "Consultation Fee is required")]
        [Range(1, 10000, ErrorMessage = "Enter valid fee")]
        public int ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}