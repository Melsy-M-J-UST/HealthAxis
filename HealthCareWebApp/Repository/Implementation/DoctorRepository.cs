using HealthCareWebApp.Data;
using HealthCareWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCareWebApp.Repository.Implementation
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly Database _db;
        public DoctorRepository(Database db)
        {
            _db = db;
        }
        public Doctor AddDoctor(Doctor doctor)
        {
            _db.Doctors.Add(doctor);
            return doctor;
        }
        public List<Doctor> GetAllDoctors()
        {
            return _db.Doctors;
        }
        public Doctor GetDoctorById(int doctorid)
        {
            var doctor = _db.Doctors.FirstOrDefault(p => p.DoctorId == doctorid);
            return doctor;
        }
        public List<Doctor> SearchDoctorBySpecialisation(Doctor.Specialisations specialisation)
        {
            var doctors = _db.Doctors.Where(doc => doc.Specialisation == specialisation).ToList();
            return doctors;
        }
        public bool UpdateDoctor(Doctor doctor)
        {
            var existing = _db.Doctors.FirstOrDefault(d => d.DoctorId == doctor.DoctorId);
            if (existing == null)
            {
                return false;
            }
            existing.DoctorName = doctor.DoctorName;
            existing.Specialisation = doctor.Specialisation;
            existing.Experience = doctor.Experience;
            existing.Fees = doctor.Fees;
            existing.IsActive = doctor.IsActive;
            return true;
        }
    }
}