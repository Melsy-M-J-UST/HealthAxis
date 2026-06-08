using HealthAxis.Api.Data;
using HealthAxis.Api.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace HealthAxis.Api.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HealthAxisEntities _context;
        public DoctorRepository(HealthAxisEntities context) { _context = context; }
        public IEnumerable<Doctor> GetAll() { return _context.Doctors.OrderBy(d => d.FullName).ToList(); }
        public IEnumerable<Doctor> GetActiveDoctors() { return _context.Doctors.Where(d => d.IsActive).OrderBy(d => d.FullName).ToList(); }
        public IEnumerable<Doctor> GetBySpecialisation(string specialisation) { return _context.Doctors.Where(d => d.Specialisation == specialisation).OrderBy(d => d.FullName).ToList(); }
        public Doctor GetById(int id) { return _context.Doctors.Find(id); }
        public Doctor Add(Doctor doctor) { _context.Doctors.Add(doctor); _context.SaveChanges(); return doctor; }
        public bool Update(Doctor doctor)
        {
            var existing = _context.Doctors.Find(doctor.DoctorId);
            if (existing == null) return false;
            existing.FullName = doctor.FullName;
            existing.Specialisation = doctor.Specialisation;
            existing.YearsOfExperience = doctor.YearsOfExperience;
            existing.ConsultationFee = doctor.ConsultationFee;
            existing.IsActive = doctor.IsActive;
            _context.SaveChanges();
            return true;
        }
        public bool ToggleStatus(int id)
        {
            var doctor = _context.Doctors.Find(id);
            if (doctor == null) return false;
            doctor.IsActive = !doctor.IsActive;
            _context.SaveChanges();
            return true;
        }
    }
}
