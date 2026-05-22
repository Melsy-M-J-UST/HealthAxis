using System;

namespace HealthAxis.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public Patient Patient { get; set; } = null!;

        public Doctor Doctor { get; set; } = null!;

        public DateTime ScheduledDate { get; set; }

        public string Slot { get; set; } = string.Empty;

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Confirmed;

        public string CancellationReason { get; set; } = string.Empty;

        public enum AppointmentStatus
        {
            Confirmed,
            Cancelled,
            Completed
        }

        public void Confirm()
        {
            if (Status == AppointmentStatus.Cancelled)
                throw new InvalidOperationException("Cannot confirm a cancelled appointment.");

            if (Status == AppointmentStatus.Completed)
                throw new InvalidOperationException("Cannot confirm a completed appointment.");

            Status = AppointmentStatus.Confirmed;
        }

        public void Cancel(string reason)
        {
            if (Status == AppointmentStatus.Confirmed)
            {
                Status = AppointmentStatus.Cancelled;
                CancellationReason = reason;
            }
            else
            {
                throw new InvalidOperationException("Only Confirmed appointments can be cancelled.");
            }
        }

        public string GetDetails()
        {
            return $"AppointmentId: {AppointmentId}, Date: {ScheduledDate}, Slot: {Slot}, Status: {Status}, Reason: {CancellationReason}";
        }
    }
}
