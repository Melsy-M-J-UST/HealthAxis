using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HealthAxis_MVC.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Specialisation { get; set; }
        public int YearsOfExperience { get; set; }
        public int ConsultationFee { get; set; }
        public bool IsActive { get; set; }
    }
}
