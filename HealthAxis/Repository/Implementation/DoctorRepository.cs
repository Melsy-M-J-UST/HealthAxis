using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repository.Implementation
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly Database _Db;

        public DoctorRepository(Database Db)
        {
            _Db = Db;
        }
        public Doctor AddDoctor(Doctor doctor)
        {
            _Db.Doctors.Add(doctor);
            return doctor;
        }
        public List<Doctor> SearchDoctorBySpecialisation(Doctor.Specialisations specialisation)
        {
            var doctors = _Db.Doctors
                .Where(doc => doc.Specialisation == specialisation)
                .ToList();

            if (!doctors.Any())
            {
                throw new DoctorNotFoundException("No doctors found with the given specialization.");
            }

            return doctors;
        }
        public Doctor? GetDoctorById(int doctorid)
        {
            var doctor = _Db.Doctors.FirstOrDefault(p => p.DoctorId == doctorid);
            if (doctor == null)
            {
                throw new DoctorNotFoundException($"Patient with id {doctorid} not registered.");
            }
            return doctor;
        }
        public List<Doctor> GetAllDoctors()
        {
            return _Db.Doctors.ToList();
        }
    }
}
