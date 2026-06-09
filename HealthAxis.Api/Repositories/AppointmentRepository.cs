using HealthAxis.Api.Data;
using HealthAxis.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HealthAxis.Api.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HealthAxisEntities _context;

        public AppointmentRepository(HealthAxisEntities context)
        {
            _context = context;
        }

        public IEnumerable<Appointment> GetAll()
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }

        public Appointment GetById(int id)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefault(a => a.AppointmentId == id);
        }

        public IEnumerable<Appointment> GetByPatient(int patientId)
        {
            return _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }

        public IEnumerable<Appointment> GetByDoctor(int doctorId)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorId == doctorId)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }

        public IEnumerable<Appointment> GetByDoctorAndDate(int doctorId, DateTime date)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Where(a =>
                    a.DoctorId == doctorId &&
                    DbFunctions.TruncateTime(a.ScheduledDate) == date.Date)
                .OrderBy(a => a.TimeSlot)
                .ToList();
        }

        public IEnumerable<Appointment> GetByDoctorAndDateRange(
            int doctorId,
            DateTime start,
            DateTime end)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate >= start &&
                    a.ScheduledDate <= end)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }

        public bool IsSlotAvailable(int doctorId, DateTime date, string slot)
        {
            return !_context.Appointments.Any(a =>
                a.DoctorId == doctorId &&
                DbFunctions.TruncateTime(a.ScheduledDate) == date.Date &&
                a.TimeSlot == slot &&
                a.Status != "Cancelled");
        }

        public Appointment Add(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return appointment;
        }

        public bool UpdateStatus(int id, string status, string reason)
        {
            var appointment = _context.Appointments.Find(id);

            if (appointment == null)
            {
                return false;
            }

            appointment.Status = status;

            appointment.CancellationReason =
                status == "Cancelled" ? reason : null;

            _context.SaveChanges();

            return true;
        }

        public bool Delete(int id)
        {
            var appointment = _context.Appointments.Find(id);

            if (appointment == null)
            {
                return false;
            }

            _context.Appointments.Remove(appointment);
            _context.SaveChanges();

            return true;
        }
    }
}