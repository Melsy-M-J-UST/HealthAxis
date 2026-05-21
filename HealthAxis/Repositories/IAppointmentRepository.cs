using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repositories
{
    public class IAppointmentRepository
    {
        Appointment BookAppointment(Appointment newAppointment);

        bool CancelAppointment(int appointmentId, string cancellationReason);

        List<Appointment> GetAppointmentsByPatient(int patientId);

        List<Appointment> GetAppointmentsByDoctor(int doctorId);

        List<Appointment> GetAllAppointments();

        Appointment? GetAppointmentById(int appointmentId);
    }
}
