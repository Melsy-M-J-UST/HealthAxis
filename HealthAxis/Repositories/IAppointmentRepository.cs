using HealthAxis.Models;

namespace HealthAxis.Repositories
{
    public interface IAppointmentRepository
    {
        Appointment Add(Appointment appointment);
        Appointment? GetById(int appointmentId);
        List<Appointment> GetAll();
        List<Appointment> GetByPatientId(int patientId);
        List<Appointment> GetByDoctorId(int doctorId);

        string? GetNextAvailableSlot(int doctorId, DateTime date);
        int GetBookedSlotCount(int doctorId, DateTime date);
        void Remove(Appointment appointment);
        bool PatientHasAppointmentAt(int patientId, DateTime date, string timeSlot);
        string? GetNextAvailableSlotAvoidingPatientConflicts(int doctorId, DateTime date, int patientId);
    }
}