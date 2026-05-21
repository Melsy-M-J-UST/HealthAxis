using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
        public DateTime ScheduledDate { get; set; }
        public string Slot { get; set; } = string.Empty;
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
        public string CancellationReason { get; set; } = string.Empty;

        public enum AppointmentStatus
        {
            Pending,
            Confirmed,
            Cancelled,
            Completed
        }
        public void Confirm()
        {
            if (Status == AppointmentStatus.Cancelled)
            {
                Console.WriteLine("Your appointment was already cancelled. Please book a new appointment");
            }
            else if (Status == AppointmentStatus.Completed)
            {
                Console.WriteLine("You have already completed the appointment. If you need another appointment, please book a new appointment.");
            }
            Status = AppointmentStatus.Confirmed;
        }
        public  string GetAppointmentSummary()
        {
            return $"Appointment ID: {AppointmentId} {Patient.GetPatientSummary} {Doctor.GetDoctorSummary} Scheduled Date: {ScheduledDate}  Time Slot: {Slot}  Status: {Status}   Cancellation Reason(if any): {CancellationReason}";
        }
    }
}
