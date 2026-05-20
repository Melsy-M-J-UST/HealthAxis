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
        public Genders Gender { get; set; }
        public DateTime RegisteredDate { get; set; }
        public int GetAge()
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;

            if (DateOfBirth > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
        public enum Genders
        {
            Male,
            Female,
            Transgender,
            Other
        };
        public string GetProfileSummary()
        {
            return $"Patient ID: {PatientId}, Name: {FullName}, Age: {GetAge()}, Gender: {Gender}, Phone: {PhoneNumber}, Email: {Email}";
        }


    }
}
