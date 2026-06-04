using HealthCareWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCareWebApp.Service.Implementation
{
    public class AppointmentService : IAppointmentService
    {
        public Appointment BookAppointment(Patient patient, Doctor doctor, DateTime date)
        {
            throw new NotImplementedException();
        }
        public bool CancelAppointment(int appointmentId, string reason)
        {
            throw new NotImplementedException();
        }
        public List<Appointment> GetAppointmentsByPatient(int patientId)
        {
            throw new NotImplementedException();
        }
        public List<Appointment> GetAppointmentsByDoctor(int doctorId)
        {
            throw new NotImplementedException();
        }
        public Appointment GetAppointmentById(int appointmentId)
        {
            throw new NotImplementedException();
        }
        public List<Appointment> GetAllAppointments()
        {
            throw new NotImplementedException();
        }
        public List<Appointment> GetUpcomingAppointments()
        {
            throw new NotImplementedException();
        }
    }
}