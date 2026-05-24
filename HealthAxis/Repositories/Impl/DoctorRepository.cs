using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthAxis.Repositories.Impl
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext _ContextDb;

        public DoctorRepository(AppDbContext contextDb)
        {
            _ContextDb = contextDb;
        }
        public Doctor AddDoctor(Doctor doctor)
        {
            _ContextDb.Doctors.Add(doctor);
            return doctor;
        }

        public Doctor? GetById(int doctorId)
        {
            return _ContextDb.Doctors
                .FirstOrDefault(d => d.DoctorId == doctorId);
        }

        public List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation)
        {
            var doctors = _ContextDb.Doctors
                .Where(doc => doc.Specialisation == specialisation)
                .ToList();
            return doctors;
        }
        public List<Doctor> GetAllDoctors()
        {
            return _ContextDb.Doctors.ToList();
        }

        public bool UpdateDoctor(Doctor doctor)
        {
            var existing = _ContextDb.Doctors.FirstOrDefault(d => d.DoctorId == doctor.DoctorId);
            if (existing == null)
            {
                return false;
            }
            existing.FullName = doctor.FullName;
            existing.Specialisation = doctor.Specialisation;
            existing.YearsOfExperience = doctor.YearsOfExperience;
            existing.ConsultationFee = doctor.ConsultationFee;
            existing.IsActive = doctor.IsActive;
            return true;
        }
    }
}
