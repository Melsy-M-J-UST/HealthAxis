using HealthAxis.Api.Data;
using HealthAxis.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Api.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HealthAxisEntities _context;

        public DoctorRepository(HealthAxisEntities context)
        {
            _context = context;
        }

        public IEnumerable<Doctor> GetAll(
            string specialisation = null,
            bool activeOnly = false)
        {
            var query = _context.Doctors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(specialisation))
            {
                query = query.Where(d => d.Specialisation == specialisation);
            }

            if (activeOnly)
            {
                query = query.Where(d => d.IsActive);
            }

            return query
                .OrderBy(d => d.FullName)
                .ToList();
        }

        public Doctor GetById(int id)
        {
            return _context.Doctors.Find(id);
        }

        public Doctor Add(Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            _context.SaveChanges();

            return doctor;
        }

        public bool Update(Doctor doctor)
        {
            var existingDoctor = _context.Doctors.Find(doctor.DoctorId);

            if (existingDoctor == null)
            {
                return false;
            }

            existingDoctor.FullName = doctor.FullName;
            existingDoctor.Specialisation = doctor.Specialisation;
            existingDoctor.YearsOfExperience = doctor.YearsOfExperience;
            existingDoctor.ConsultationFee = doctor.ConsultationFee;
            existingDoctor.IsActive = doctor.IsActive;

            _context.SaveChanges();

            return true;
        }

        public bool ToggleStatus(int id)
        {
            var doctor = _context.Doctors.Find(id);

            if (doctor == null)
            {
                return false;
            }

            doctor.IsActive = !doctor.IsActive;

            _context.SaveChanges();

            return true;
        }

        public int GetUpcomingAppointmentCount(int doctorId)
        {
            return _context.Appointments.Count(a =>
                a.DoctorId == doctorId &&
                a.ScheduledDate >= DateTime.Today &&
                a.Status != "Cancelled");
        }
    }
}