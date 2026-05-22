using System;
using System.Collections.Generic;
using System.Text;
using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Exceptions;
using HealthAxis.Data;

namespace HealthAxis.Repositories.Impl
{
    public class AppointmentRepository : IAppointmentRepository
    {

        private readonly _ContextDb _dbContext;
        public AppointmentRepository(_ContextDb dbContext)
        {
            _dbContext = dbContext;
        }
        public Appointment BookAppointment(Patient patient, Doctor doctor, DateTime date, string slot)
        {
            if (!Enum.TryParse(slot, true, out Appointment.TimeSlot slotname))
            {
                throw new AppointmentConflictException("Invalid Time slot");
            }
            Appointment newappointment = new Appointment { Patient = patient, Doctor = doctor, ScheduledDate = date, Slot = slotname };
            _dbContext.Appointments.Add(newappointment);
            return newappointment;
        }

        //CANCELLING APPOINTMENT
        public bool CancelAppointment(string appointmentid, string reason)
        {
            var appointment = _dbContext.Appointments.FirstOrDefault(app => app.Appointment_id == appointmentid);
            if (appointment == null)
            {
                throw new AppointmentConflictException("Appointment id doesn't exist");
            }
            appointment.CancellationReason = reason;
            appointment.Status = Appointment.AppointmentStatus.Cancelled;
            return true;
        }

        //FETCHING APPOINTMENTS VIA PATIENT ID
        public List<Appointment> GetAppointmentsByPatient(int patientid)
        {
            List<Appointment> appointmentbypatientid = _ContextDb.Appointments.Where(app => app.Patient.PatientId == patientid).ToList();
            if (appointmentbypatientid.Count == 0)
            {
                throw new AppointmentConflictException("Appointment with this Patient ID not found");
            }
            return appointmentbypatientid;

        }

        //FETCHING APPOINTMENTS VIA DOCTOR ID
        public List<Appointment> GetAppointmentsByDoctor(int doctorid)
        {
            var appointmentbydoctorid = _dbContext.Appointments.Where(app => app.Doctor.DoctorId == doctorid).ToList();
            if (appointmentbydoctorid.Count == 0)
            {
                throw new AppointmentConflictException("Appointment with this Doctor ID not found");
            }
            return appointmentbydoctorid;
        }

        //FETCHING APPOINTMENTS
        public List<Appointment> GetUpcomingAppointments()
        {
            if (_dbContext.Appointments.Count == 0)
            {
                throw new AppointmentConflictException("No Appointments!");
            }
            return _dbContext.Appointments;
        }

        //FETCHING APPOINTMENT VIA APPOINTMENT ID
        public Appointment? GetAppointmentById(string appointmentid)
        {
            var appointment = _dbContext.Appointments.FirstOrDefault(app => app.Appointment_id == appointmentid);
            if (appointment == null)
            {
                throw new AppointmentConflictException("Appointment with this ID doesn't exist");
            }
            return appointment;
        }
    }
}
