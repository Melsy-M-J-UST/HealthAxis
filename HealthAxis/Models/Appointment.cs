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

        public String Slot { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        public string CancellationReason { get; set; } = string.Empty;

        public enum AppointmentStatus
        {
            Pending,
            Confirmed,
            Cancelled,
            Completed
        }

        public override string ToString()
        {
            return $"AppointmentId: {AppointmentId}, Patient: {Patient.GetProfileSummary}, Doctor: {Doctor.GetScheduleSummary}, ScheduledDate: {ScheduledDate},Time Slot: {Slot}, Status: {Status}";
        }
    }
}
