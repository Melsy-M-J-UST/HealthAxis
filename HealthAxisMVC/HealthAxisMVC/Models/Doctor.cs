using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthAxisMVC.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        public SpecialisationOption Specialisation { get; set; }
        public int YearsOfExperience { get; set; }
        public int ConsultationFee { get; set; }
        public bool IsActive { get; set; }

        public enum SpecialisationOption
        {
            GeneralPractitioner,
            Cardiologist,
            Dermatologist,
            Neurologist,
            Pediatrician,
            Psychiatrist,
            OrthopedicSurgeon,
            Gynecologist,
            Oncologist,
            Endocrinologist
        }
    }
}