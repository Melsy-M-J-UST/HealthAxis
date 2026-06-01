using System;

namespace HealthAxis.Models
{
    public class HealthRecord
    {
        public int HealthRecordId { get; set; }

        public Patient Patient { get; set; } = null!;

        public Doctor Doctor { get; set; } = null!;

        public DateTime VisitDate { get; set; }

        public string Diagnosis { get; set; } = string.Empty;

        public string Prescription { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        public Appointment Appointment { get; set; } = null!;

        public string GetRecordSummary()
        {
            return $"HealthRecordId: {HealthRecordId}, Patient: {Patient.GetProfileSummary()}, Doctor: {Doctor.GetProfileSummary()}, VisitDate: {VisitDate.ToShortDateString()}, Diagnosis: {Diagnosis}, Prescription: {Prescription}, Notes: {Notes}";
        }
    }
}