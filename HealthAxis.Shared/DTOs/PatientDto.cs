using HealthAxis.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace HealthAxis.Shared.DTOs
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        [Required, StringLength(50)] public string FullName { get; set; }
        [Required, DataType(DataType.Date)] public DateTime DateOfBirth { get; set; }
        [Required] public GenderEnum Gender { get; set; }
        [Required, RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits.")] public string PhoneNumber { get; set; }
        [Required, EmailAddress, StringLength(50)] public string Email { get; set; }
        [StringLength(10)] public string InsuranceID { get; set; }
        public DateTime CreatedDate { get; set; }
        public int AppointmentCount { get; set; }
    }
}
