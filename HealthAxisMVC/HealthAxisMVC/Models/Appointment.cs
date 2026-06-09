using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthAxisMVC.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string TimeSlot { get; set; } = string.Empty;
        public StatusOption Status { get; set; } = StatusOption.Pending;
        public string CancellationReason { get; set; } = string.Empty;

        public enum StatusOption
        {
            Pending,
            Confirmed,
            Cancelled,
            Completed
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
    }
}