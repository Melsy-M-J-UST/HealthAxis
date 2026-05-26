using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Services
{
    public interface IAppointmentService
    {
        Appointment BookAppointment(Patient patient, Doctor doctor,DateTime date);
        bool CancelAppointment(int appointmentId, string reason);
        List<Appointment> GetAppointmentsByPatient(int patientId);
        List<Appointment> GetAppointmentsByDoctor(int doctorId);
        List<Appointment> GetUpcomingAppointments();

        List<Appointment> GetAll();

        Appointment? GetById(int id);
      
    }
}
