using HealthAxis.Models;

namespace HealthAxis.Service
{
    public interface IAppointmentService
    {
        Appointment BookAppointment(Appointment newAppointment);
        bool CancelAppointment(int appointmentId, string reason);
        List<Appointment> GetAppointmentsBypatient(int patientId);
        List<Appointment> GetAppointmentsByDoctor(int doctorId);

        Appointment? GetAppointmentById(int appointmentId);
        List<Appointment> GetAllAppointments();
    }
}