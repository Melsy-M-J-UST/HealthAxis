using HealthAxis.Models;
using HealthAxis.Data;
using HealthAxis.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Repositories.Impl
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly Database _dbContext;

        public AppointmentRepository(Database dbContext)
        {
            _dbContext = dbContext;
        }

        public Appointment? GetAppointmentById(int appointmentId)
        {
            return _dbContext.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
        }


        public bool PatientHasAppointmentAt(int patientId, DateTime date, string Slot)

        {

            return _dbContext.Appointments.Any(a =>
                a.Patient.PatientId == patientId &&
                a.ScheduledDate.Date == date.Date &&
                string.Equals(a.Slot, Slot, StringComparison.OrdinalIgnoreCase) &&
                a.Status != Appointment.AppointmentStatus.Cancelled);

        }
        public string GetNextAvailableSlot(int doctorId, DateTime date)
        {
            var bookedSlots = _dbContext.Appointments
                .Where(a =>
                    a.Doctor.DoctorId == doctorId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.Status != Appointment.AppointmentStatus.Cancelled)
                .Select(a => a.Slot)
                .ToList();

            foreach (var slot in _dbContext.DailySlots)
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

        public Appointment AddAppointment(Appointment appointment)
        {
            appointment.AppointmentId = _dbContext.GetNextAppointmentId();
            _dbContext.Appointments.Add(appointment);
            appointment.Doctor.Appointments.Add(appointment);
            return appointment;
        }

        public List<Appointment> GetByPatientId(int patientId)
        {
            return _dbContext.Appointments
                .Where(a => a.Patient.PatientId == patientId)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.Slot)
                .ToList();
        }

        public List<Appointment> GetByDoctorId(int doctorId)
        {
            return _dbContext.Appointments
                .Where(a => a.Doctor.DoctorId == doctorId)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.Slot)
                .ToList();
        }

        public void Remove(Appointment appointment)

        {

            if (appointment == null) return;


            _dbContext.Appointments.Remove(appointment);

            if (appointment.Doctor != null)

            {

                appointment.Doctor.Appointments.RemoveAll(a => a.AppointmentId == appointment.AppointmentId);

            }

        }

        public bool CancelAppointment(int appointmentId, string reason)
        {
            var appointment = GetAppointmentById(appointmentId);
            if (appointment == null || appointment.Status == Appointment.AppointmentStatus.Cancelled)
            {
                return false;
            }
            appointment.Status = Appointment.AppointmentStatus.Cancelled;
            appointment.CancellationReason = reason;
            return true;
        }
        public int GetBookedSlotCount(int doctorId, DateTime date)
        {
            return _dbContext.Appointments.Count(a =>
                a.Doctor.DoctorId == doctorId &&
                a.ScheduledDate.Date == date.Date &&
                a.Status != Appointment.AppointmentStatus.Cancelled);
        }
        public List<Appointment> GetAllAppointments()
        {
            return _dbContext.Appointments
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.Slot)
                .ToList();
        }

        public string? GetNextAvailableSlotAvoidingPatientConflicts(int doctorId, DateTime date, int patientId)
        {
            var bookedSlotsForDoctor = _dbContext.Appointments
                .Where(a =>
                    a.Doctor.DoctorId == doctorId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.Status != Appointment.AppointmentStatus.Cancelled)
                .Select(a => a.Slot)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var patientBookedSlots = _dbContext.Appointments
                .Where(a =>
                    a.Patient.PatientId == patientId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.Status != Appointment.AppointmentStatus.Cancelled)
                .Select(a => a.Slot)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var slot in _dbContext.DailySlots)
            {
                if (bookedSlotsForDoctor.Contains(slot)) continue;
                if (patientBookedSlots.Contains(slot)) continue;
                return slot;
            }

            return null;
        }
    }
}