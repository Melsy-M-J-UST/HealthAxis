using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs
{
    public class HealthRecordDto
    {
        public int RecordId { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; }

        public int DoctorId { get; set; }

        public string DoctorName { get; set; }

        public string DoctorSpecialisation { get; set; }

        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "Diagnosis is required.")]
        [StringLength(500)]
        public string Diagnosis { get; set; }

        [Required(ErrorMessage = "Prescription is required.")]
        [StringLength(500)]
        public string Prescription { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }

        public int? AppointmentId { get; set; }
    }
}