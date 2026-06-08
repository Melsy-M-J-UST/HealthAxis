using HealthAxis.Api.Data;
using HealthAxis.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
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
                .Include("Patient")
                .Include("Doctor")
                .OrderByDescending(a => a.ScheduledDate)
                .ToList();
        }

        public Appointment GetById(int id)
        {
            return _context.Appointments.Find(id);
        }

        public IEnumerable<Appointment> GetByPatient(int patientId)
        {
            return _context.Appointments
                .Include("Doctor")
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }

        public IEnumerable<Appointment> GetByDoctor(int doctorId)
        {
            return _context.Appointments
                .Include("Patient")
                .Where(a => a.DoctorId == doctorId)
                .OrderByDescending(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }

        public IEnumerable<Appointment> GetByDoctorAndDate(int doctorId, DateTime date)
        {
            return _context.Appointments
                .Include("Patient")
                .Where(a => a.DoctorId == doctorId && a.ScheduledDate == date)
                .OrderBy(a => a.TimeSlot)
                .ToList();
        }

        public bool IsSlotAvailable(int doctorId, DateTime date, string timeSlot)
        {
            return !_context.Appointments.Any(a =>
                a.DoctorId == doctorId &&
                a.ScheduledDate == date &&
                a.TimeSlot == timeSlot &&
                a.Status != "Cancelled");
        }

        public Appointment Add(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return appointment;
        }

        public bool UpdateStatus(int appointmentId, string status, string cancellationReason)
        {
            var appointment = _context.Appointments.Find(appointmentId);

            if (appointment == null)
            {
                return false;
            }

            appointment.Status = status;
            appointment.CancellationReason = cancellationReason;

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