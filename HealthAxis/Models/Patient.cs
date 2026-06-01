using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public GenderOptions Gender { get; set; } = GenderOptions.Other;

        public string PhoneNumber { get; set; } = string.Empty;

        public string? Email { get; set; } = string.Empty;

        public string? InsuranceID { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public enum GenderOptions
        {
            Male,
            Female,
            Transgender,
            Other
        };

        public int GetAge()
        {
            var today = DateTime.Today;

            var age = today.Year - DateOfBirth.Year;

            if (DateOfBirth.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

        public string GetProfileSummary()
        {
            return $"PatientId: {PatientId}, FullName: {FullName}, Age : {GetAge()}, Gender: {Gender}, PhoneNumber: {PhoneNumber}, Email: {Email}";
        }
    }
}
