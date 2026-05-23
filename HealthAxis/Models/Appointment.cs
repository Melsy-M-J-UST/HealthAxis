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
        public StatusOption Status { get; set; } = StatusOption.Confirmed;
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
        public void Cancel(string reason)
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
        }

        public enum StatusOption
        {
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
