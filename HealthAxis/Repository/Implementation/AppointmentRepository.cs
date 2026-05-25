using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repository.Implementation
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly Database _Db;
        public AppointmentRepository(Database db)
        {
            _Db = db;
        }

        public Appointment AddAppointment(Appointment appointment)
        {
            appointment.AppointmentId = _Db.GetNextAppointmentId();
            _Db.Appointments.Add(appointment);
            appointment.Doctor.Appointments.Add(appointment);
            return appointment;
        }

        public bool CancelAppointment(int appointmentid, string reason)
        {
            var appointment = _Db.Appointments.FirstOrDefault(app => app.AppointmentId == appointmentid);
            if (appointment == null)
            {
                throw new AppointmentNotFoundException($"Appointment with id {appointmentid} is not found.");
            }
            appointment.CancellationReason = reason;
            appointment.Status = Appointment.AppointmentStatus.Cancelled;
            return true;
        }

        public List<Appointment> GetAppointmentsByPatient(int patientid)
        {
            List<Appointment> appointmentbypatientid = _Db.Appointments.Where(app => app.Patient.PatientId == patientid).OrderBy(a => a.ScheduledDate).ThenBy(a => a.Slot).ToList();
            return appointmentbypatientid;
        }

        public List<Appointment> GetAppointmentsByDoctor(int doctorid)
        {
            var appointmentbydoctorid = _Db.Appointments.Where(app => app.Doctor.DoctorId == doctorid).OrderBy(a => a.ScheduledDate).ThenBy(a => a.Slot).ToList();
            return appointmentbydoctorid;
        }

        public List<Appointment> GetAllAppointments()
        {
            return _Db.Appointments.OrderBy(a => a.ScheduledDate).ThenBy(a => a.Slot).ToList();
        }

        public Appointment? GetAppointmentById(int appointmentid)
        {
            var appointment = _Db.Appointments.FirstOrDefault(app => app.AppointmentId == appointmentid);
            return appointment;
        }
        public string? GetNextAvailableSlot(int doctorId, DateTime date)
        {
            var bookedSlots = _Db.Appointments
                .Where(a =>
                    a.Doctor.DoctorId == doctorId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.Status != Appointment.AppointmentStatus.Cancelled)
                .Select(a => a.Slot)
                .ToList();

            foreach (var slot in _Db.DailySlots)
            {
                bool isSlotBooked = bookedSlots.Any(bookedSlot =>
                    bookedSlot.Equals(slot, StringComparison.OrdinalIgnoreCase));

                if (!isSlotBooked)
                {
                    return slot;
                }
            }
            return null;
        }

        public int GetBookedSlotCount(int doctorId, DateTime date)
        {
            return _Db.Appointments.Count(a =>
                a.Doctor.DoctorId == doctorId &&
                a.ScheduledDate.Date == date.Date &&
                a.Status != Appointment.AppointmentStatus.Cancelled);
        }

        public void Remove(Appointment appointment)
        {
            if (appointment == null) return;

            _Db.Appointments.Remove(appointment);
            if (appointment.Doctor != null)
            {
                appointment.Doctor.Appointments.RemoveAll(a => a.AppointmentId == appointment.AppointmentId);
            }
        }
        public bool PatientHasAppointmentAt(int patientId, DateTime date, string timeSlot)
        {
            return _Db.Appointments.Any(a =>
                a.Patient.PatientId == patientId &&
                a.ScheduledDate.Date == date.Date &&
                string.Equals(a.Slot, timeSlot, StringComparison.OrdinalIgnoreCase) &&
                a.Status != Appointment.AppointmentStatus.Cancelled);
        }

        public string? GetNextAvailableSlotAvoidingPatientConflicts(int doctorId, DateTime date, int patientId)
        {
            var bookedSlotsForDoctor = _Db.Appointments
                .Where(a =>
                    a.Doctor.DoctorId == doctorId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.Status != Appointment.AppointmentStatus.Cancelled)
                .Select(a => a.Slot)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var patientBookedSlots = _Db.Appointments
                .Where(a =>
                    a.Patient.PatientId == patientId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.Status != Appointment.AppointmentStatus.Cancelled)
                .Select(a => a.Slot)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var slot in _Db.DailySlots)
            {
                if (bookedSlotsForDoctor.Contains(slot)) continue;
                if (patientBookedSlots.Contains(slot)) continue;
                return slot;
            }

            return null;
        }
    }
}
