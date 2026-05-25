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
            var doctors = _Db.Doctors.Where(doc => doc.Specialisation == specialisation).ToList();
            return doctors;
        }
        public Doctor? GetDoctorById(int doctorid)
        {
            var doctor = _Db.Doctors.FirstOrDefault(p => p.DoctorId == doctorid);
            return doctor;
        }
        public List<Doctor> GetAllDoctors()
        {
            return _Db.Doctors.ToList();
        }

        public bool UpdateDoctor(Doctor doctor)
        {
            var existing = _Db.Doctors.FirstOrDefault(d => d.DoctorId == doctor.DoctorId);
            if (existing == null)
            {
                return false;
            }
            existing.DoctorName = doctor.DoctorName;
            existing.Specialisation = doctor.Specialisation;
            existing.Experience = doctor.Experience;
            existing.Fees = doctor.Fees;
            existing.IsPractising = doctor.IsPractising;
            return true;
        }
    }
}
