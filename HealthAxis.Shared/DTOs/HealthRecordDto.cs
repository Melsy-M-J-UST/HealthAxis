using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs
{
    public class HealthRecordDto
    {
        public int RecordId { get; set; }

        [Required(ErrorMessage = "Appointment ID is required.")]
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "Diagnosis is required.")]
        [StringLength(500)]
        public string Diagnosis { get; set; }

        [StringLength(1000)]
        public string Prescription { get; set; }

        [StringLength(2000)]
        public string Notes { get; set; }
    }
}