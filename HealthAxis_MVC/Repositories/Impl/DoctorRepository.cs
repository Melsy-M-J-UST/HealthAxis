using HealthAxis_MVC.Exceptions;
using HealthAxis_MVC.Models;
using HealthAxis_MVC.Database;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis_MVC.Repositories.Impl
{
    public class DoctorRepository : IDoctorRepository
    {
        public void AddDoctor(Doctor doctor)
        {
            if (AppContextDB.Doctors.Any())
            {
                doctor.DoctorId = AppContextDB.Doctors.Max(d => d.DoctorId) + 1;
            }
            else
            {
                doctor.DoctorId = 1;
            }

            AppContextDB.Doctors.Add(doctor);
        }

        public List<Doctor> GetAllDoctors()
        {
            return AppContextDB.Doctors;
        }

        public Doctor GetById(int id)
        {
            return AppContextDB.Doctors.Single(x => x.DoctorId == id);
        }

        public void UpdateDoctor(int id, Doctor doctor)
        {
            var existingDoctor = AppContextDB.Doctors.First(x => x.DoctorId == id);
            existingDoctor.FullName = doctor.FullName;
            existingDoctor.Specialisation = doctor.Specialisation;
            existingDoctor.ConsultationFee = doctor.ConsultationFee;
            existingDoctor.IsActive = doctor.IsActive;
            existingDoctor.YearsOfExperience = doctor.YearsOfExperience;
        }
    }
}

      