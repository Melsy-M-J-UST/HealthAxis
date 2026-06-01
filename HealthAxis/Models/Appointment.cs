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

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Confirmed;

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
               Console.WriteLine("Cannot confirm a cancelled appointment.");
               return;
            }
            if (Status == AppointmentStatus.Completed)
            {
                Console.WriteLine("Cannot confirm a completed appointment.");
                return;
            }
            Status = AppointmentStatus.Confirmed;
            Console.WriteLine("Appointment confirmed.");
        }

        public void Cancel(string reason)
        {
            if (Status == AppointmentStatus.Completed)
            {
                Console.WriteLine("Completed appointments cannot be cancelled");
                return;
            }

            Status = AppointmentStatus.Cancelled;
            CancellationReason = reason;
            Console.WriteLine("Appointment cancelled.");
        }
        public void Complete()
        {
            if (Status == AppointmentStatus.Cancelled)
            {
                Console.WriteLine("Cancelled appointments cannot be completed");
                return;
            }
            if (Status != AppointmentStatus.Confirmed)
            {
                Console.WriteLine("Only confirmed appointments can be completed.");
                return;
            }

            Status = AppointmentStatus.Completed;
        }

        public string? GetDetails(List<Appointment> allAppointments)
        {
            return $"Appointment ID: {AppointmentId} \n{Patient.GetProfileSummary()} \n{Doctor.GetScheduleSummary(allAppointments)}\n Scheduled Date: {ScheduledDate:dd-MM-yyyy} \n Time Slot: {Slot}  Status: {Status} \n  Cancellation reason(if any): {CancellationReason}";
        }
    }
}
