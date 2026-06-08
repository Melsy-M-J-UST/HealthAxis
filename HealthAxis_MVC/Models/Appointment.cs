using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis_MVC.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Please select a patient")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Please select a doctor")]
        public int DoctorId { get; set; }

        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

        [Required(ErrorMessage = "Please select a date")]
        public DateTime? ScheduledDate { get; set; }

        [Required(ErrorMessage = "Please select a slot")]
        public SlotType Slot { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        [Required(ErrorMessage = "Please select specialisation")]
        public Doctor.SpecialisationType? Specialisation { get; set; }

        public enum SlotType
        {
            Slot1,
            Slot2,
            Slot3,
            Slot4,
            Slot5
        }

        public enum AppointmentStatus
        {
            Pending,
            Confirmed,
            Cancelled,
            Completed
        }
    }
}