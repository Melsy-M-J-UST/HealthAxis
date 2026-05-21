using HAP_Pod4_ConsoleApp_au.Models;

namespace HAP_Pod4_ConsoleApp_au.Services
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