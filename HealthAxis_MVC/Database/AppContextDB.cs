using HealthAxis_MVC.Models;
using System;
using System.Collections.Generic;
using static HealthAxis_MVC.Models.Doctor;

namespace HealthAxis_MVC.Database
{
    public class AppContextDB
    {
        public static List<Doctor> Doctors { get; set; }
        public static List<Patient> Patients { get; set; }

        public static List<HealthRecord> Records { get; set; }

        static AppContextDB()
        {
            Doctors = new List<Doctor>
             {
                 new Doctor { DoctorId = 1, FullName = "John", Specialisation = SpecialisationType.Cardiology, YearsOfExperience = 4, ConsultationFee = 1000, IsActive = true },
                 new Doctor { DoctorId = 2, FullName = "Jane", Specialisation = SpecialisationType.Neurology, YearsOfExperience = 4, ConsultationFee = 1500, IsActive = true },
                 new Doctor { DoctorId = 3, FullName = "Jack", Specialisation = SpecialisationType.Orthopedics, YearsOfExperience = 6, ConsultationFee = 1100, IsActive = true },
                 new Doctor { DoctorId = 4, FullName = "Jerry", Specialisation = SpecialisationType.Pediatrics, YearsOfExperience = 5, ConsultationFee = 1000, IsActive = true },
                 new Doctor { DoctorId = 5, FullName = "Jia", Specialisation = SpecialisationType.GeneralPractitioner, YearsOfExperience = 4, ConsultationFee = 1200, IsActive = true }
             };

            Patients = new List<Patient>
            {
                new Patient { PatientId = 1, FullName = "Asha", DateOfBirth = DateTime.Now.AddYears(-25), Gender = "Female", PhoneNumber = "9876543210", Email = "asha@test.com", CreatedDate = DateTime.Now },
                new Patient { PatientId = 2, FullName = "Rahul", DateOfBirth = DateTime.Now.AddYears(-30), Gender = "Male", PhoneNumber = "9123456780", Email = "rahul@test.com", CreatedDate = DateTime.Now },
                new Patient { PatientId = 3, FullName = "Meera", DateOfBirth = DateTime.Now.AddYears(-40), Gender = "Female", PhoneNumber = "9000000000", Email = "meera@test.com", CreatedDate = DateTime.Now }
            };
            Records = new List<HealthRecord>();
        }
    }
}