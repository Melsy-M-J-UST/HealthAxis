using HealthAxis.Shared.Models;
using HealthAxisWebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxisWebApp.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HealthAxisDBEntities db;

        public DoctorRepository()
        {
            db = new HealthAxisDBEntities();
        }

        public List<Doctor> GetAll()
        {
            return db.Doctors.ToList();
        }

        public List<Doctor> GetAll(string sortBy, string specialisation)
        {
            var query = db.Doctors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(specialisation) &&
                specialisation != "all")
            {
                if (int.TryParse(specialisation, out int specValue))
                {
                    query = query.Where(d => d.Specialisation == specValue);
                }
            }

            if (sortBy == "id")
            {
                query = query.OrderBy(d => d.DoctorId);
            }
            else if (sortBy == "name_desc")
            {
                query = query.OrderByDescending(d => d.FullName);
            }
            else
            {
                query = query.OrderBy(d => d.FullName);
            }

            return query.ToList();
        }

        public Doctor GetById(int id)
        {
            return db.Doctors.Find(id);
        }

        public void Add(Doctor doctor)
        {
            db.Doctors.Add(doctor);
            db.SaveChanges();
        }

        public void Update(Doctor doctor)
        {
            db.Entry(doctor).State =
                System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
        }

        public int GetUpcomingAppointmentCount(int doctorId)
        {
            return db.Appointments.Count(a =>
                a.DoctorId == doctorId &&
                a.ScheduledDate >= DateTime.Today);
        }

        public void Delete(int id)
        {
            Doctor doctor = db.Doctors.Find(id);

            if (doctor != null)
            {
                db.Doctors.Remove(doctor);
                db.SaveChanges();
            }
        }
    }
}
