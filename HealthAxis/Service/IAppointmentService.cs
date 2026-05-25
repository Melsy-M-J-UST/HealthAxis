using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Service
{
    public interface IAppointmentService
    {
        Appointment BookAppointment(Patient patient, Doctor doctor, DateTime date);
        bool CancelAppointment(int appointmentId, string reason);
        List<Appointment> GetAppointmentsByPatient(int patientId);
        List<Appointment> GetAppointmentsByDoctor(int doctorId);
        Appointment? GetAppointmentById(int appointmentId);
        List<Appointment> GetAllAppointments();
        List<Appointment> GetUpcomingAppointments();
    }
}
