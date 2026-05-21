using HAP_Pod4_ConsoleApp_au.Models;
using System;
using System.Collections.Generic;

namespace HAP_Pod4_ConsoleApp_au.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly List<Appointment> _appointments;

        public AppointmentRepository()
        {
            _appointments = new List<Appointment>();
        }

        public Appointment BookAppointment(Appointment newAppointment)
        {
            _appointments.Add(newAppointment);

            return newAppointment;
        }

        public bool CancelAppointment(int appointmentid, string reason)
        {
            Appointment? appointment =
                GetAppointmentById(appointmentid);

            if (appointment == null)
            {
                return false;
            }

            appointment.Cancel(reason);

            return true;
        }

        public List<Appointment> GetAppointmentsByPatient(int patientid)
        {
            List<Appointment> patientAppointments =
                new List<Appointment>();

            foreach (var appointment in _appointments)
            {
                if (appointment.Patient.PatientId == patientid)
                {
                    patientAppointments.Add(appointment);
                }
            }

            return patientAppointments;
        }

        public List<Appointment> GetAppointmentsByDoctor(int doctorid)
        {
            List<Appointment> doctorAppointments =
                new List<Appointment>();

            foreach (var appointment in _appointments)
            {
                if (appointment.Doctor.DoctorId == doctorid)
                {
                    doctorAppointments.Add(appointment);
                }
            }

            return doctorAppointments;
        }

        public List<Appointment> GetAllAppointments()
        {
            return _appointments;
        }

        public Appointment? GetAppointmentById(int appointmentid)
        {
            foreach (var appointment in _appointments)
            {
                if (appointment.AppointmentId == appointmentid)
                {
                    return appointment;
                }
            }

            return null;
        }
    }
}