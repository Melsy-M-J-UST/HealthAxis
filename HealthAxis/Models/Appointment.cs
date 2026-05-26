using HealthAxis.Services;
using HealthAxis.Models;
using System;

namespace HealthAxis.Models
{

    public class Appointment
    {
        public int AppointmentId { get; set; }
        public Patient Patient { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
        public DateTime ScheduledDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        // New appointments are created as Pending. Pending appointments reserve slots until confirmed.
        public StatusOption Status { get; set; } = StatusOption.Pending;
        public string CancellationReason { get; set; } = string.Empty;

        public void Confirm()
        {
            if (Status == StatusOption.Cancelled)
            {
                Console.WriteLine("Cancelled appointments cannot be confirmed.");
                return;
            }
            if (Status == StatusOption.Completed)
            {
                Console.WriteLine("Appointment already completed. Cannot be confirmed.");
                return;
            }

            Status = StatusOption.Confirmed;
            Console.WriteLine("Appointment confirmed.");

        }
        public void Cancel(string reason)
        {
            if (Status == StatusOption.Completed)
            {
                Console.WriteLine("Completed appointments cannot be cancelled");
                return;
            }

            Status = StatusOption.Cancelled;
            CancellationReason = reason;
            Console.WriteLine("Appointment cancelled.");
        }

        public void Complete()
        {
            if (Status == StatusOption.Cancelled)
            {
                Console.WriteLine("Cancelled appointments cannot be completed");
                return;
            }

            Status = StatusOption.Completed;
            Console.WriteLine("Appointment marked as completed.");
        }

        public enum StatusOption
        {
            Pending,
            Confirmed,
            Cancelled,
            Completed
        }

        public string GetDetails(List<Appointment> allAppointments)
        {
            return $"\n=============================== \nAppointment ID: {AppointmentId} \n{Patient.GetProfileSummary()} \n{Doctor.GetScheduleSummary(allAppointments)}\n Scheduled Date: {ScheduledDate:dd-MM-yyyy} \n Time Slot: {TimeSlot}  Status: {Status} \n  Cancellation reason(if any): {CancellationReason}\n ===============================\n";
        }

    }
}
