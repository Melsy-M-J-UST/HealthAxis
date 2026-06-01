using HealthAxis.Models;

namespace HealthAxis.Repositories
{
    public interface IAppointmentRepository
    {

        List<Appointment> GetByPatientId(int patientId);

        List<Appointment> GetByDoctorId(int doctorId);
        bool PatientHasAppointmentAt(int patientId, DateTime date, string slot);
        void Remove(Appointment appointment);

        Appointment AddAppointment(Appointment appointment);

        string? GetNextAvailableSlot(int doctorId, DateTime date);

        int GetBookedSlotCount(int doctorId, DateTime date);

        Appointment? GetAppointmentById(int appointmentId);

        List<Appointment> GetAllAppointments();

        string? GetNextAvailableSlotAvoidingPatientConflicts(int doctorId, DateTime date, int patientId);

    }
}