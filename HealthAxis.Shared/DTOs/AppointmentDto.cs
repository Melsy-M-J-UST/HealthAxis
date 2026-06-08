using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public string PatientName { get; set; }

        [Required(ErrorMessage = "Doctor is required.")]
        public int DoctorId { get; set; }

        public string DoctorName { get; set; }

        public string DoctorSpecialisation { get; set; }

        [Required(ErrorMessage = "Scheduled date is required.")]
        [DataType(DataType.Date)]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Time slot is required.")]
        public string TimeSlot { get; set; }

        public string Status { get; set; }

        public string CancellationReason { get; set; }
    }
}