using HealthAxis.Exceptions;
using HealthAxis.Data;
using HealthAxis.Models;
using HealthAxis.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;


namespace HealthAxis.Repositories.Impl
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _dbContext;
        public AppointmentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Appointment BookAppointment(Appointment newAppointment)
        {
            _dbContext.Appointments.Add(newAppointment);
            return newAppointment;
        }


        public bool CancelAppointment(int appointmentid, string reason)
        {
            var appointment = _dbContext.Appointments.FirstOrDefault(app => app.Appointment_id == appointmentid);
            if (appointment == null)
            {
                throw new AppointmentConflictException("Appointment id doesn't exist");
            }
            appointment.CancellationReason = reason;
            appointment.Status = Appointment.StatusOption.Cancelled;
            return true;
        }


        public List<Appointment> GetAppointmentsByPatient(int patientid)
        {
            List<Appointment> appointmentbypatientid = _dbContext.Appointments.Where(app => app.Patient.PatientId == patientid).ToList();
            if (appointmentbypatientid.Count == 0)
            {
                throw new AppointmentConflictException("Appointment with this Patient ID not found");
            }
            return appointmentbypatientid;

        }


        public List<Appointment> GetAppointmentsByDoctor(int doctorid)
        {
            var appointmentbydoctorid = _dbContext.Appointments.Where(app => app.Doctor.DoctorId == doctorid).ToList();
            if (appointmentbydoctorid.Count == 0)
            {
                throw new AppointmentConflictException("Appointment with this Doctor ID not found");
            }
            return appointmentbydoctorid;
        }


        public List<Appointment> GetAllAppointments()
        {
            if (_dbContext.Appointments.Count == 0)
            {
                throw new AppointmentConflictException("No Appointments!");
            }
            return _dbContext.Appointments;
        }


        public Appointment? GetAppointmentById(int appointmentid)
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
