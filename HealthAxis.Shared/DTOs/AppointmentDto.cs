using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Patient ID is required.")]
        public int PatientId { get; set; }

        public string PatientName { get; set; }

        [Required(ErrorMessage = "Doctor ID is required.")]
        public int DoctorId { get; set; }

        public string DoctorName { get; set; }

        [Required(ErrorMessage = "Scheduled Date is required.")]
        public DateTime ScheduledDate { get; set; }

        [Required(ErrorMessage = "Time Slot is required.")]
        public int TimeSlot { get; set; }

        public string TimeSlotName { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [Range(0, 3, ErrorMessage = "Status must be 0, 1, 2, or 3.")]
        public int Status { get; set; }

        public string StatusName { get; set; }

        public string CancellationReason { get; set; }
    }
}