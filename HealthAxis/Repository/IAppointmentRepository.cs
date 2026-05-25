using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace HealthAxis.Repository
{
    public interface IAppointmentRepository
    {
        Appointment AddAppointment(Appointment appointment);
        bool CancelAppointment(int appointmentid, string reason);
        List<Appointment> GetAppointmentsByPatient(int patientid);
        List<Appointment> GetAppointmentsByDoctor(int doctorid);
        List<Appointment> GetAllAppointments();
        Appointment? GetAppointmentById(int appointmentid);
        string? GetNextAvailableSlot(int doctorId, DateTime date);
        int GetBookedSlotCount(int doctorId, DateTime date);
        void Remove(Appointment appointment);
        bool PatientHasAppointmentAt(int patientId, DateTime date, string timeSlot);
        string? GetNextAvailableSlotAvoidingPatientConflicts(int doctorId, DateTime date, int patientId);

    }
}
