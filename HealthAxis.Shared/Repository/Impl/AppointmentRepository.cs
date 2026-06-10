using HealthAxis.Shared.Models;
using HealthAxisWebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HealthAxisWebApp.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HealthAxisDBEntities db;

        public AppointmentRepository()
        {
            db = new HealthAxisDBEntities();
        }

        public List<Appointment> GetAll()
        {
            return db.Appointments
                     .Include(a => a.Patient)
                     .Include(a => a.Doctor)
                     .ToList();
        }

        public Appointment GetById(int id)
        {
            return db.Appointments
                     .Include(a => a.Patient)
                     .Include(a => a.Doctor)
                     .FirstOrDefault(a =>
                         a.AppointmentId == id);
        }

        public void Add(Appointment appointment)
        {
            db.Appointments.Add(appointment);
            db.SaveChanges();
        }

        public void Update(Appointment appointment)
        {
            db.Entry(appointment).State =
                EntityState.Modified;

            db.SaveChanges();
        }
        public bool IsSlotAvailable(int doctorId, DateTime date, int timeSlot)
        {
            return !db.Appointments.Any(a =>
                a.DoctorId == doctorId &&
                a.ScheduledDate == date &&
                a.TimeSlot == timeSlot &&
                a.Status != 2); // not cancelled
        }

        public List<Appointment> GetByPatientId(int patientId)
        {
            return db.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.ScheduledDate)
                .ToList();
        }

        public List<Appointment> GetTodayAppointments(int doctorId)
        {
            return db.Appointments
                .Include(a => a.Patient)
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate == System.DateTime.Today)
                .ToList();
        }

        public List<Appointment> GetWeeklyAppointments(int doctorId)
        {
            var today = System.DateTime.Today;
            var weekEnd = today.AddDays(7);

            return db.Appointments
                .Include(a => a.Patient)
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate >= today &&
                    a.ScheduledDate <= weekEnd)
                .ToList();
        }


        public void Delete(int id)
        {
            Appointment appointment =
                db.Appointments.Find(id);

            if (appointment != null)
            {
                db.Appointments.Remove(appointment);
                db.SaveChanges();
            }
        }
    }

}
