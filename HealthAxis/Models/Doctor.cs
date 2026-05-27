using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HealthAxis.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public SpecialisationOption Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public int ConsultationFee { get; set; }

        public bool IsActive { get; set; }

        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

        public enum SpecialisationOption
        {
            GeneralPractitioner,
            Cardiologist,
            Dermatologist,
            Neurologist,
            Pediatrician,
            Psychiatrist,
            OrthopedicSurgeon,
            Gynecologist,
            Oncologist,
            Endocrinologist
        }
        

        public bool IsAvailable(DateTime date)
        {
            if (!IsActive)
                return false;
            int booked = Appointments.Count(a => a.ScheduledDate.Date == date.Date && a.Status != Appointment.StatusOption.Cancelled);


            const int capacity = 5;

            return booked < capacity;
        }


        public string GetScheduleSummary(List<Appointment> allAppointments)
        {
            int upcomingCount = allAppointments.Count(a =>
                a.Doctor.DoctorId == DoctorId &&
                a.ScheduledDate.Date >= DateTime.Today &&
                a.Status == Appointment.StatusOption.Confirmed
            );

            return $"Dr. {FullName} ({Specialisation}) - Upcoming Appointments: {upcomingCount}";
        }


        public string GetProfileSummary()
        {
            return $"\n===============================\nDoctorId: {DoctorId},\n FullName: Dr. {FullName},\n Specialisation: {Specialisation},\n YearsOfExperience: {YearsOfExperience},\n ConsultationFee: {ConsultationFee},\n IsActive: {IsActive}\n===============================\n";
        }

    }
}
