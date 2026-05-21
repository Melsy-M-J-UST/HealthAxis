using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

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

        public string IsAvailableForConsultation()
        {
            return IsActive ? "Available for consultation" : "Not available for consultation";
        }
        public string GetScheduleSummary()
        {
            int upcomingCount = 0; // This would be calculated based on actual appointments in a real implementation
            return $"DoctorId: {DoctorId}, FullName: {FullName}, Specialisation: {Specialisation}, UpcomingAppointments: {upcomingCount}";
        }
        public string GetProfileSummary()
        {
            return $"DoctorId: {DoctorId}, FullName: {FullName}, Specialisation: {Specialisation}, YearsOfExperience: {YearsOfExperience}, ConsultationFee: {ConsultationFee}, IsActive: {IsActive}";
        }
    }
}
