using HealthAxis.Models;
using System;

namespace HealthAxis.Models
{

    public class Appointment
    {
        public int Appointment_id { get; set; }
        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
        public DateTime ScheduledDate { get; set; }
        public TimeSlot Slot { get; set; }
        public StatusOption Status { get; set; } = StatusOption.Pending;
        public string CancellationReason { get; set; } = string.Empty;

        public void Confirm()
        {
            if (Status == StatusOption.Cancelled)
            {
                Console.WriteLine("Cancelled appointments cannot be confirmed.");
            }
            else if (Status == StatusOption.Completed)
            {
                Console.WriteLine("Appointment already Completed. Cannot be confirmed.");
            }
            Status = StatusOption.Confirmed;

        }
        public enum TimeSlot
        {
            Slot1 = 1,
            Slot2 = 2,
            Slot3 = 3,
            Slot4 = 4,
            Slot5 = 5
        }

        public enum StatusOption
        {
            Pending,
            Confirmed,
            Cancelled,
            Completed
        }

        /*public void Cancel(string reason)
        {
            if (Status == StatusOption.Completed)
            {
                Console.WriteLine("Completed appointments cannot be cancelled");
            }
            Status = StatusOption.Cancelled;
            CancellationReason = reason;
        }

        public void Complete()
        {
            if (Status == StatusOption.Cancelled)
            {
                Console.WriteLine("Cancelled appointments cannot be completed");
            }
            Status = StatusOption.Completed;
        }*/

        public override string ToString()
        {
            return $"Appointment ID: {Appointment_id} {Patient.GetProfileSummary} {Doctor.GetScheduleSummary} Scheduled Date: {ScheduledDate}  Time Slot: {Slot}  Status: {Status}   Cancellation reason(if any): {CancellationReason}";
        }

    }
}
