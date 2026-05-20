using System;
using System.Collections.Generic;
using System.Text;

namespace HAP_Pod4_ConsoleApp_au.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public Patient Patient { get; set; }

        public Doctor Doctor { get; set; }

        public DateTime ScheduledDate { get; set; }

        public TimeSlotOption TimeSlot { get; set; }

        public StatusOption Status { get; set; } = StatusOption.Pending;

        public string CancellationReason { get; set; } = string.Empty;

        // ENUM FOR APPOINTMENT STATUS
        public enum StatusOption
        {
            Pending,
            Confirmed,
            Cancelled,
            Completed
        }

        // ENUM FOR TIME SLOTS
        public enum TimeSlotOption
        {
            TenAMToTwelvePM,
            TwelvePMToTwoPM,
            TwoPMToFourPM,
            FourPMToSixPM
        }

        // CONFIRM APPOINTMENT
        public void Confirm()
        {
            if (Status == StatusOption.Cancelled)
            {
                Console.WriteLine("Cancelled appointment cannot be confirmed.");
                return;
            }

            Status = StatusOption.Confirmed;
        }

        // CANCEL APPOINTMENT
        public void Cancel(string reason)
        {
            if (Status == StatusOption.Completed)
            {
                Console.WriteLine("Completed appointment cannot be cancelled.");
                return;
            }

            Status = StatusOption.Cancelled;
            CancellationReason = reason;
        }

        // COMPLETE APPOINTMENT
        public void Complete()
        {
            if (Status != StatusOption.Confirmed)
            {
                Console.WriteLine("Only confirmed appointments can be completed.");
                return;
            }

            Status = StatusOption.Completed;
        }

        // FORMATTED TIME SLOT
        public string GetFormattedTimeSlot()
        {
            switch (TimeSlot)
            {
                case TimeSlotOption.TenAMToTwelvePM:
                    return "10 AM - 12 PM";

                case TimeSlotOption.TwelvePMToTwoPM:
                    return "12 PM - 2 PM";

                case TimeSlotOption.TwoPMToFourPM:
                    return "2 PM - 4 PM";

                case TimeSlotOption.FourPMToSixPM:
                    return "4 PM - 6 PM";

                default:
                    return "Invalid Time Slot";
            }
        }

        // APPOINTMENT SUMMARY
        public string GetDetails()
        {
            return $"Appointment ID: {AppointmentId}\n" +
                   $"Patient Name: {Patient.FullName}\n" +
                   $"Doctor Name: Dr. {Doctor.FullName}\n" +
                   $"Specialisation: {Doctor.Specialisation}\n" +
                   $"Scheduled Date: {ScheduledDate.ToShortDateString()}\n" +
                   $"Time Slot: {GetFormattedTimeSlot()}\n" +
                   $"Status: {Status}\n" +
                   $"Cancellation Reason: {CancellationReason}";
        }
    }
}