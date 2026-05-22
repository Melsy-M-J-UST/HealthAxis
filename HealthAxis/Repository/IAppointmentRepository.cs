using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repository
{
    public interface IAppointmentRepository
    {
        Appointment BookAppointment(Appointment newAppointment);
        bool CancelAppointment(int appointmentid, string reason);
        List<Appointment> GetAppointmentsByPatient(int patientid);
        List<Appointment> GetAppointmentsByDoctor(int doctorid);
        List<Appointment> GetAllAppointments();
        Appointment? GetAppointmentById(int appointmentid);
    }
}
