using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCareWebApp.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string InsuranceId { get; set; }
        public Genders Gender { get; set; }
        public DateTime RegisteredDate { get; set; }
        public enum Genders
        {
            Male,
            Female,
            Transgender,
            Other
        };
    }
}