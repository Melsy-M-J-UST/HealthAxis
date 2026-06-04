using HealthCareWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthCareWebApp.Repository.Implementation
{
    public class AppointmentRepository : IAppointmentRepository
    {
        public Appointment AddAppointment(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public bool CancelAppointment(int appointmentid, string reason)
        {
            throw new NotImplementedException();
        }

        public List<Appointment> GetAllAppointments()
        {
            throw new NotImplementedException();
        }

        public Appointment GetAppointmentById(int appointmentid)
        {
            throw new NotImplementedException();
        }

        public List<Appointment> GetAppointmentsByDoctor(int doctorid)
        {
            throw new NotImplementedException();
        }

        public List<Appointment> GetAppointmentsByPatient(int patientid)
        {
            throw new NotImplementedException();
        }

        public int GetBookedSlotCount(int doctorId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public string GetNextAvailableSlot(int doctorId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public string GetNextAvailableSlotAvoidingPatientConflicts(int doctorId, DateTime date, int patientId)
        {
            throw new NotImplementedException();
        }

        public bool PatientHasAppointmentAt(int patientId, DateTime date, string timeSlot)
        {
            throw new NotImplementedException();
        }

        public void Remove(Appointment appointment)
        {
            throw new NotImplementedException();
        }
    }
}