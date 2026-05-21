using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Models
{
    public class HealthRecord
    {
        public int RecordId { get; set; }
        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
        public DateTime VisitedDate { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string Prescription { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string GetHealthRecordSummary()
        {
            return $"Record ID: {RecordId}, Patient: {Patient.PatientName}, Doctor: {Doctor.DoctorName}, Visit Date: {VisitedDate.ToShortDateString()}, Diagnosis: {Diagnosis}, Prescription: {Prescription}";
        }
    }
}
