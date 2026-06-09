using HealthAxisMVC.Database;
using HealthAxisMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthAxisMVC.Repositories.Impl
{
    public class DoctorRepository : IDoctorRepository
    {
        public void AddDoctor(Doctor doctor)
        {
            AppDB.Doctors.Add(doctor);
        }

        public List<Doctor> GetAllDoctors()
        {
            return AppDB.Doctors;
        }

        public Doctor GetById(int doctorId)
        {
            var doctor = AppDB.Doctors.FirstOrDefault(d => d.DoctorId == doctorId);
            return doctor;
        }

        public List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation)
        {
            var doctorsBySpecialisation = AppDB.Doctors.FindAll(d => d.Specialisation == specialisation);
            return doctorsBySpecialisation;
        }

        public void UpdateDoctor(int id, Doctor doctor)
        {
            var existingDoctor = AppDB.Doctors.First(d => d.DoctorId == id);
            existingDoctor.FullName = doctor.FullName;
            existingDoctor.Specialisation = doctor.Specialisation;
            existingDoctor.YearsOfExperience = doctor.YearsOfExperience;
            existingDoctor.ConsultationFee = doctor.ConsultationFee;
            existingDoctor.IsActive = doctor.IsActive;
        }
    }
}