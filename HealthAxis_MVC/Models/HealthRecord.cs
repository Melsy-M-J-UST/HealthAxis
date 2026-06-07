using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis_MVC.Models
{
    public class HealthRecord
    {
        public int HealthRecordId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        public int AppointmentId { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }

        [Required]
        public string Diagnosis { get; set; }

        [Required]
        public string Prescription { get; set; }

        public string Notes { get; set; }
    }
}