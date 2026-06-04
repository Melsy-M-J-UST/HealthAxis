using System;
using System.Collections.Generic;
using HealthAxis_MVC.Models;
using System.Linq;
using System.Web;

namespace HealthAxis_MVC.Database
{
    public class AppContextDB
    {
        public static List<Doctor> Doctors { get; set; }

        static AppContextDB()
        {
            Doctors = new List<Doctor>
            {
                new Doctor { DoctorId = 1, FullName = "John", Specialisation = "Cardiologist", YearsOfExperience = 4, ConsultationFee = 1000, IsActive = true },
                new Doctor { DoctorId = 2, FullName = "Jane", Specialisation = "General", YearsOfExperience = 4, ConsultationFee = 1500, IsActive = true },
                new Doctor { DoctorId = 3, FullName = "Jack", Specialisation = "Ongologist", YearsOfExperience = 6, ConsultationFee = 1100, IsActive = true },
                new Doctor { DoctorId = 4, FullName = "Jerry", Specialisation = "General", YearsOfExperience = 5, ConsultationFee = 1000, IsActive = true },
                new Doctor { DoctorId = 5, FullName = "Jia", Specialisation = "General", YearsOfExperience = 4, ConsultationFee = 1200, IsActive = true }

            };
        }
    }
}