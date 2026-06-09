using HealthAxisMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAxisMVC.Services
{
    public interface IAppointmentService
    {
        Appointment BookAppointment(Patient patient, Doctor doctor, DateTime date);
        void CancelAppointment(int appointmentId, string reason);
        List<Appointment> GetAppointmentsByPatient(int patientId);
        List<Appointment> GetAppointmentsByDoctor(int doctorId);
        List<Appointment> GetUpcomingAppointments();
        Appointment GetAppointmentById(int appointmentId);
        List<Appointment> GetAllAppointments();
        void CompleteAppointment(int id);
        void ConfirmAppointment(int id);
    }
}
