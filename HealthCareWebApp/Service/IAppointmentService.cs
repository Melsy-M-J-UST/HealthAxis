using HealthCareWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareWebApp.Service
{
    public interface IAppointmentService
    {
        Appointment BookAppointment(Patient patient, Doctor doctor, DateTime date);
        bool CancelAppointment(int appointmentId, string reason);
        List<Appointment> GetAppointmentsByPatient(int patientId);
        List<Appointment> GetAppointmentsByDoctor(int doctorId);
        Appointment GetAppointmentById(int appointmentId);
        List<Appointment> GetAllAppointments();
        List<Appointment> GetUpcomingAppointments();
    }
}
