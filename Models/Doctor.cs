using System;
using System.Collections.Generic;
using System.Text;

namespace HAP_Pod4_ConsoleApp_au.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public SpecialisationOption Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public int ConsultationFee { get; set; }

        public bool IsActive { get; set; }

        List<Appointment> Appointments { get; set; } = new List<Appointment>();

        public enum SpecialisationOption
        {
            GeneralPractitioner = 1,
            Cardiologist = 2,
            Dermatologist = 3,
            Neurologist = 4,
            Pediatrician = 5,
            Psychiatrist = 6,
            OrthopedicSurgeon = 7,
            Gynecologist = 8,
            Oncologist = 9,
            Endocrinologist = 10
        }


        public bool IsAvailable()
        {
            return IsActive;
        }

        public string GetScheduleSummary()
        {
            int UpcomingCount = 0;

            foreach (var a in Appointments)
            {
                if (a.ScheduledDate > DateTime.Now && a.Status == Appointment.StatusOption.Confirmed)
                {
                    UpcomingCount++;
                }
            }
            return $"Dr. {FullName} ({Specialisation}) - Upcoming Appointments: {UpcomingCount}";
        }

        public string GetProfileSummary()
        {
            return $"DoctorId: {DoctorId}, \nFullName: {FullName}, \nSpecialisation: {Specialisation}, \nYearsOfExperience: {YearsOfExperience}, \nConsultationFee: {ConsultationFee}, \nIsActive: {IsActive}";
        }

    }

}
