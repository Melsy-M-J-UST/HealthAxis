using HealthAxis_MVC.Database;
using HealthAxis_MVC.Models;
using System.Collections.Generic;
using System.Linq;

public class AppointmentRepository
{
    public void Add(Appointment appointment)
    {
        appointment.AppointmentId =
            AppContextDB.Appointments.Any()
            ? AppContextDB.Appointments.Max(a => a.AppointmentId) + 1
            : 1;

        appointment.Patient = AppContextDB.Patients.First(p => p.PatientId == appointment.PatientId);
        appointment.Doctor = AppContextDB.Doctors.First(d => d.DoctorId == appointment.DoctorId);

        appointment.Status = Appointment.AppointmentStatus.Pending;

        AppContextDB.Appointments.Add(appointment);
    }

    public List<Appointment> GetAll()
    {
        return AppContextDB.Appointments;
    }

    public void UpdateStatus(int id, Appointment.AppointmentStatus status)
    {
        var appt = AppContextDB.Appointments.First(a => a.AppointmentId == id);

        if (appt.Status == Appointment.AppointmentStatus.Pending)
        {
            appt.Status = status;
        }
    }
}