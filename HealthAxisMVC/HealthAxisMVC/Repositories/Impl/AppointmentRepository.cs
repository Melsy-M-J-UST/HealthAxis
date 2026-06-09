using HealthAxisMVC.Database;
using HealthAxisMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthAxisMVC.Repositories.Impl
{
    public class AppointmentRepository : IAppointmentRepository
    {
        public Appointment Add(Appointment appointment)
        {
            appointment.AppointmentId = AppDB.GetNextAppointmentId();
            AppDB.Appointments.Add(appointment);


            return appointment;
        }

        public List<Appointment> GetAll()
        {
            return AppDB.Appointments
                            .OrderBy(a => a.ScheduledDate)
                            .ThenBy(a => a.TimeSlot)
                            .ToList();
        }

        public int GetBookedSlotCount(int doctorId, DateTime date)
        {
            return AppDB.Appointments.Count(a =>
                a.Doctor.DoctorId == doctorId &&
                a.ScheduledDate.Date == date.Date &&
                a.Status != Appointment.StatusOption.Cancelled);
        }

        public List<Appointment> GetByDoctorId(int doctorId)
        {
            return AppDB.Appointments
                .Where(a => a.Doctor.DoctorId == doctorId)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }

        public Appointment GetById(int appointmentId)
        {
            return AppDB.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
        }

        public List<Appointment> GetByPatientId(int patientId)
        {
            return AppDB.Appointments
                .Where(a => a.Patient.PatientId == patientId)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }

        public string GetNextAvailableSlot(int doctorId, DateTime date)
        {
            var bookedSlots = AppDB.Appointments
                .Where(a =>
                    a.Doctor.DoctorId == doctorId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.Status != Appointment.StatusOption.Cancelled)
                .Select(a => a.TimeSlot)
                .ToList();

            foreach (var slot in AppDB.DailySlots)
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

        public string GetNextAvailableSlotAvoidingPatientConflicts(int doctorId, DateTime date, int patientId)
        {
            var bookedSlotsForDoctor = AppDB.Appointments
                .Where(a =>
                    a.Doctor.DoctorId == doctorId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.Status != Appointment.StatusOption.Cancelled)
                .Select(a => a.TimeSlot)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var patientBookedSlots = AppDB.Appointments
                .Where(a =>
                    a.Patient.PatientId == patientId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.Status != Appointment.StatusOption.Cancelled)
                .Select(a => a.TimeSlot)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var slot in AppDB.DailySlots)
            {
                if (bookedSlotsForDoctor.Contains(slot)) continue;
                if (patientBookedSlots.Contains(slot)) continue;
                return slot;
            }

            return null;
        }

        public bool PatientHasAppointmentAt(int patientId, DateTime date, string timeSlot)
        {
            return AppDB.Appointments.Any(a =>
                a.Patient.PatientId == patientId &&
                a.ScheduledDate.Date == date.Date &&
                string.Equals(a.TimeSlot, timeSlot, StringComparison.OrdinalIgnoreCase) &&
                a.Status != Appointment.StatusOption.Cancelled);
        }

        public void Remove(Appointment appointment)
        {
            if (appointment == null) return;

            AppDB.Appointments.Remove(appointment);
        }
    }
}