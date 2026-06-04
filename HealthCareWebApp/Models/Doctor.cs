using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCareWebApp.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public Specialisations Specialisation { get; set; }
        public int Experience { get; set; }
        public int Fees { get; set; }
        public bool IsActive { get; set; }
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public enum Specialisations
        {
            GeneralPractitioner,
            Cardiologist,
            Dermatologist,
            Endocrinologist,
            Gynecologist,
            Neurologist,
            Oncologist,
            OrthopedicSurgeon,
            Pediatrician,
            Psychiatrist
        }
    }
}