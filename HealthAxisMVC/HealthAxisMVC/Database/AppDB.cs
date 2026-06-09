using HealthAxisMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthAxisMVC.Database
{
    public static class AppDB
    {
        public static List<Patient> Patients { get; set; }
        public static List<Doctor> Doctors { get; set; }
        public static List<Appointment> Appointments { get; set; }

        public static List<string> DailySlots { get; set; }

        public static int GetNextAppointmentId()
        {
            if (Appointments.Count == 0)
                return 1;

            return Appointments.Max(a => a.AppointmentId) + 1;
        }


        static AppDB()
        {
            Patients = new List<Patient>
            {
                new Patient{PatientId=1, FullName = "John", DateOfBirth=new DateTime(2003,12,30), Email="John@gmail.com", Gender=Patient.GenderOptions.Male, CreatedDate=DateTime.Now, InsuranceId="INS0001", PhoneNumber="9887767857" },
                new Patient{PatientId=2, FullName = "Jim", DateOfBirth=new DateTime(2003,1,30), Email="Jim@gmail.com", Gender=Patient.GenderOptions.Male, CreatedDate=DateTime.Now, InsuranceId="INS0002", PhoneNumber="927764557" }

            };

            Doctors = new List<Doctor>
            {
                new Doctor{DoctorId=1, FullName = "Tom", Specialisation=Doctor.SpecialisationOption.Dermatologist, YearsOfExperience=3, ConsultationFee=1000, IsActive=true},
                new Doctor{DoctorId=2, FullName = "Mike", Specialisation=Doctor.SpecialisationOption.Cardiologist, YearsOfExperience=5, ConsultationFee=1500, IsActive=true},
            };
            Appointments = new List<Appointment>();


            DailySlots = new List<string>
            {
            "09:00 AM",
            "10:00 AM",
            "11:00 AM",
            "02:00 PM",
            "03:00 PM"
            };
        }
    }
}