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
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public List<DayOfWeek>? SurgeryDays { get; set; } = new List<DayOfWeek>();
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
        public bool IsAvailable(DateTime date)
        {
            if (!IsPractising)
                return false;
            int booked = Appointments.Count(a => a.ScheduledDate.Date == date.Date && a.Status != Appointment.AppointmentStatus.Cancelled);
            const int capacity = 5;
            return booked < capacity;
        }
        public string GetScheduleSummary(List<Appointment> allAppointments)
        {
            int upcomingCount = allAppointments.Count(a =>
                a.Doctor.DoctorId == DoctorId &&
                a.ScheduledDate.Date >= DateTime.Today &&
                a.Status == Appointment.AppointmentStatus.Confirmed
            );
            return $"Dr. {DoctorName} ({Specialisation}) - Upcoming Appointments: {upcomingCount}";
        }
        public string GetDoctorSummary()
        {
            return $"DoctorId: {DoctorId}, FullName: {DoctorName}, Specialisation: {Specialisation}, Experience(in years): {Experience}, Consultation Fee: {Fees}";
        }

    }
}
