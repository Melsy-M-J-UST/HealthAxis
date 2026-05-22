using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Services
{
    public class IAppointmentService
    {
        Appointment BookAppointment(Appointment newAppointment);
        bool CancelAppointment(int appointmentId, string reason);
        List<Appointment> GetAppointmentsBypatient(int patientId);
        List<Appointment> GetAppointmentsByDoctor(int doctorId);

        Appointment? GetAppointmentById(int appointmentId);
        List<Appointment> GetAllAppointments();
    }
}
