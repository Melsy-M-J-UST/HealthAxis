using System;

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

        public Appointment(string appointmentId, DateTime scheduledDate, string slot)
        {
            this.AppointmentId = appointmentId;
            this.ScheduledDate = scheduledDate;
            this.Slot = slot;
            this.Status = AppointmentStatus.Pending;
            CancellationReason = string.Empty;
        }

        public void Confirm()
        {
            if (Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot confirm a cancelled appointment.");
            }
        else if (Status == AppointmentStatus.Completed)
            {
                throw new InvalidOperationException("Cannot confirm a completed appointment.");
            }
            Status = AppointmentStatus.Confirmed;
        }

        public void Cancel(string reason)
        {
            if (Status == AppointmentStatus.Confirmed || Status == AppointmentStatus.Pending)
            {
                Status = AppointmentStatus.Cancelled;
                CancellationReason = reason;
            }
            else
            {
                throw new InvalidOperationException("Only pending or confirmed appointments can be cancelled.");
            }
        }

        public void Complete()
        {
            if (Status != AppointmentStatus.Confirmed)
            {
                throw new InvalidOperationException("Only confirmed appointments can be completed.");
            }
            else if (Status == AppointmentStatus.Cancelled)
            {
                throw new InvalidOperationException("Cannot complete a cancelled appointment.");
            }
            Status = AppointmentStatus.Completed;
        }
       
        public string GetDetails()
        {
            return $"Appointment ID: {AppointmentId} Scheduled Date: {ScheduledDate} Time Slot: {slot} Status: {status} Cancellation Reason(if any): {CancellationReason}";
        }

    }
}
