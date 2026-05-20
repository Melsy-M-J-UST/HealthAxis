using HAP_Pod4_ConsoleApp_au.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentPortal.ConsoleApp.Repositories
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
