using HealthAxis.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs
{
    public class HealthRecordDto
    {
        public int RecordId { get; set; }
        [Range(1, int.MaxValue)] public int PatientId { get; set; }
        public string PatientName { get; set; }
        [Range(1, int.MaxValue)] public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public SpecialisationEnum? DoctorSpecialisation { get; set; }
        public DateTime VisitDate { get; set; }
        [Required, StringLength(500)] public string Diagnosis { get; set; }
        [Required, StringLength(500)] public string Prescription { get; set; }
        [StringLength(1000)] public string Notes { get; set; }
    }
}
