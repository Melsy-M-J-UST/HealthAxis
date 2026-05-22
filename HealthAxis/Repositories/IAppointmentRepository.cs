using HealthAxis.Models;

public interface IAppointmentRepository
{
    Appointment BookAppointment(Patient patient, Doctor doctor, DateTime date, string slot);

    bool CancelAppointment(int appointmentId, string cancellationReason);

    List<Appointment> GetAppointmentsByPatient(int patientId);

    List<Appointment> GetAppointmentsByDoctor(int doctorId);

    List<Appointment> GetUpcomingAppointments();

    Appointment? GetAppointmentById(int appointmentId);

    List<Appointment> GetAllAppointments(); // ✅ ADDED
}
