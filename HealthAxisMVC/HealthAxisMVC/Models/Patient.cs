using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace HealthAxisMVC.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        public GenderOptions Gender { get; set; } = GenderOptions.Other;

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; }

        public string InsuranceId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public enum GenderOptions
        {
            Male,
            Female,
            Transgender,
            Other
        }
    }
}