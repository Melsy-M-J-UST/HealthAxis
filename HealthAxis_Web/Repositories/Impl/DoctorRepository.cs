using HealthAxis_Web.Models;
using HealthAxis_Web.Database;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis_MVC.Repositories.Impl
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDBContext _context;

        public DoctorRepository(AppDBContext context)
        {
            _context = context;
        }
        public Doctor AddDoctor(Doctor doctor)
        {
           _context.Doctors.Add(doctor);
            _context.SaveChanges();
            return doctor;
        }

        public List<Doctor> GetAllDoctors()
        {
            return _context.Doctors.ToList();
        }

        public Doctor GetById(int id)
        {
            return _context.Doctors.FirstOrDefault(x => x.DoctorId == id);
        }

        public Doctor UpdateDoctor(int id, Doctor doctor)
        {
            var existingDoctor = _context.Doctors.FirstOrDefault(x => x.DoctorId == id);
            if (existingDoctor == null)
            {
                return null;
            }
            existingDoctor.FullName = doctor.FullName;
            existingDoctor.Specialisation = doctor.Specialisation;
            existingDoctor.ConsultationFee = doctor.ConsultationFee;
            existingDoctor.IsActive = doctor.IsActive;
            existingDoctor.YearsOfExperience = doctor.YearsOfExperience;
            _context.SaveChanges();
            return existingDoctor;
        }
    }
}

      