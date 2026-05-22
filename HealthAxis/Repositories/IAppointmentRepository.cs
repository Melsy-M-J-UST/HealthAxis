using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repositories
{
    public interface IAppointmentRepository
    {
        Appointment BookAppointment(Patient patient,Doctor doctor,DateTime date,String slot);

        bool CancelAppointment(int appointmentId, string cancellationReason);

        List<Appointment> GetAppointmentsByPatient(int patientId);

        List<Appointment> GetAppointmentsByDoctor(int doctorId);

        List<Appointment> GetUpcomingAppointments();

        Appointment? GetAppointmentById(int appointmentId);
    }
}
