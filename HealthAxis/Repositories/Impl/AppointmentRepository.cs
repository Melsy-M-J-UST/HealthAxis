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

        public Appointment BookAppointment(Patient patient, Doctor doctor, DateTime date, string slot)
        {
            var newAppointment = new Appointment
            {
                AppointmentId = _dbContext.GetNextAppointmentId(),
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = date,
                Slot = slot
            };

            _dbContext.Appointments.Add(newAppointment);
            return newAppointment;
        }

        public bool CancelAppointment(int appointmentId, string reason)
        {
            var appointment = _dbContext.Appointments
                .FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (appointment == null)
                throw new AppointmentConflictException("Appointment not found");

            appointment.Cancel(reason);
            return true;
        }

        public List<Appointment> GetAppointmentsByPatient(int patientId)
        {
            var list = _dbContext.Appointments
                .Where(a => a.Patient.PatientId == patientId)
                .ToList();

            if (!list.Any())
                throw new AppointmentConflictException("No appointments for patient");

            return list;
        }

        public List<Appointment> GetAppointmentsByDoctor(int doctorId)
        {
            var list = _dbContext.Appointments
                .Where(a => a.Doctor.DoctorId == doctorId)
                .ToList();

            if (!list.Any())
                throw new AppointmentConflictException("No appointments for doctor");

            return list;
        }

        public List<Appointment> GetUpcomingAppointments()
        {
            return _dbContext.Appointments
                .Where(a => a.ScheduledDate >= System.DateTime.Now)
                .ToList();
        }

        public Appointment? GetAppointmentById(int appointmentId)
        {
            return _dbContext.Appointments
                .FirstOrDefault(a => a.AppointmentId == appointmentId);
        }

        public List<Appointment> GetAllAppointments()
        {
            return _dbContext.Appointments;
        }
    }
}