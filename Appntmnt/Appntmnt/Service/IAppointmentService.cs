using Appntmnt.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Appntmnt.Service
{
    public interface IAppointmentService
    {
        Appointment BookAppointment(Patient patient, Doctor doctor, DateTime date);
        bool CancelAppointment(int appointmentId, string reason);
        List<Appointment> GetAppointmentsByPatient(int patientId);
        List<Appointment> GetAppointmentsByDoctor(int doctorId);
        List<Appointment> GetUpcomingAppointments();
        Appointment? GetAppointmentById(int appointmentId);
        List<Appointment> GetAllAppointments();
    }
}
