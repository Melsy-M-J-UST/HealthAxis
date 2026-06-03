using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Appntmnt.Models
{
    [ExcludeFromCodeCoverage]
    public class Patient
    {
        public int PatientId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public GenderOptions Gender { get; set; } = GenderOptions.Other;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? InsuranceId { get; set; }
        public DateTime CreatedDate { get; set; }

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

        public enum GenderOptions
        {
            Male,
            Female,
            Transgender,
            Other
        };

        public string GetProfileSummary()
        {
            return $"\n===============================\n Patient ID: {PatientId}, \n Name: {FullName},\n Age: {GetAge()},\n Gender: {Gender},\n Phone: {PhoneNumber}, \n Email: {Email},\n Created Date: {CreatedDate}\n===============================\n";
        }
    }
}
