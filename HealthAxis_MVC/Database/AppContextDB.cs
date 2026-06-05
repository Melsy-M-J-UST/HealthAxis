using System;
using System.Collections.Generic;
using HealthAxis_MVC.Models;

namespace HealthAxis_MVC.Database
{
    public class AppContextDB
    {
        public static List<Doctor> Doctors { get; set; }
        public static List<Patient> Patients { get; set; }

        static AppContextDB()
        {
            Doctors = new List<Doctor>
            {
                new Doctor { DoctorId = 1, FullName = "John", Specialisation = "Cardiologist", YearsOfExperience = 4, ConsultationFee = 1000, IsActive = true },
                new Doctor { DoctorId = 2, FullName = "Jane", Specialisation = "General", YearsOfExperience = 4, ConsultationFee = 1500, IsActive = true },
                new Doctor { DoctorId = 3, FullName = "Jack", Specialisation = "Oncologist", YearsOfExperience = 6, ConsultationFee = 1100, IsActive = true },
                new Doctor { DoctorId = 4, FullName = "Jerry", Specialisation = "General", YearsOfExperience = 5, ConsultationFee = 1000, IsActive = true },
                new Doctor { DoctorId = 5, FullName = "Jia", Specialisation = "General", YearsOfExperience = 4, ConsultationFee = 1200, IsActive = true }
            };

            Patients = new List<Patient>
            {
                new Patient { PatientId = 1, FullName = "Asha", DateOfBirth = DateTime.Now.AddYears(-25), Gender = "Female", PhoneNumber = "9876543210", Email = "asha@test.com", CreatedDate = DateTime.Now },
                new Patient { PatientId = 2, FullName = "Rahul", DateOfBirth = DateTime.Now.AddYears(-30), Gender = "Male", PhoneNumber = "9123456780", Email = "rahul@test.com", CreatedDate = DateTime.Now },
                new Patient { PatientId = 3, FullName = "Meera", DateOfBirth = DateTime.Now.AddYears(-40), Gender = "Female", PhoneNumber = "9000000000", Email = "meera@test.com", CreatedDate = DateTime.Now }
            };
        }
    }
}