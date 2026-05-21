using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public Specialisations Specialisation { get; set; }
        public int Experience { get; set; }
        public int Fees { get; set; }
        public bool IsPractising { get; set; }

        public enum Specialisations
        {
            GeneralPractitioner,
            Cardiologist,
            Dermatologist,
            Endocrinologist,
            Gynecologist,
            Neurologist,
            Oncologist,
            OrthopedicSurgeon,
            Pediatrician,
            Psychiatrist 
        }
        //public string GetTodaysScheduledCount()
        //{
        //    int TodaysCount = 0;

        //    foreach (var a in Appointments)
        //    {
        //        if (a.ScheduledDate == DateTime.Date && a.Status == Appointment.StatusOption.Confirmed)
        //        {
        //            TodaysCount++;
        //        }
        //    }
        //    return $"Dr. {FullName} ({Specialisation}) - Today's Appointments: {TodaysCount}";
        //}
        public string GetDoctorSummary()
        {
            return $"DoctorId: {DoctorId}, FullName: {DoctorName}, Specialisation: {Specialisation}, Experience(in years): {Experience}, Consultation Fee: {Fees}";
        }

    }
}
