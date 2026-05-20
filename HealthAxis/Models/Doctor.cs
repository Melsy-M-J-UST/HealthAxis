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

        public string GetProfileSummary()
        {
            return $"DoctorId: {DoctorId}, FullName: {FullName}, Specialisation: {Specialisation}, YearsOfExperience: {YearsOfExperience}, ConsultationFee: {ConsultationFee}, IsActive: {IsActive}";
        }
    }
}
