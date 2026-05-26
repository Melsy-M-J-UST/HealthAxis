using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Models
{
    public class HealthRecord
    {
        public int RecordId { get; set; }
        public Appointment Appointment { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
        public DateTime VisitDate { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string Prescription { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        public string GetSummary()
        {
            return $"Record ID: {RecordId}, Patient: {Patient.FullName}, Doctor: {Doctor.FullName}, Visit Date: {VisitDate.ToShortDateString()}, Diagnosis: {Diagnosis}, Prescription: {Prescription}";
        }
    }
}