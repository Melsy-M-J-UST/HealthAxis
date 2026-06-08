using HealthAxis_MVC.Models;
using System.Collections.Generic;

public interface IAppointmentRepository
{
    void Add(Appointment appointment);
    List<Appointment> GetAll();
    List<Appointment> GetByDoctor(int doctorId);
    List<Appointment> GetByPatient(int patientId);
    void UpdateStatus(int id, Appointment.AppointmentStatus status);
}