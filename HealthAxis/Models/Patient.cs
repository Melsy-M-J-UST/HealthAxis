using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string? FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? InsuranceId { get; set; }


    }
}
