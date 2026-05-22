using System;
using System.Collections.Generic;
using System.Text;
using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Exceptions;
using HealthAxis.Data;

namespace HealthAxis.Repositories.Impl
{
    public class AppointmentRepository : IAppointmentRepository
    {
        public Appointment BookAppointment(Patient patient, Doctor doctor, DateTime date, string slot)
        {
            if (!Enum.TryParse(slot, true, out Appointment.TimeSlot slotname))
            {
                throw new AppointmentConflictException("Invalid Time slot");
            }
            Appointment newappointment = new Appointment { Patient = patient, Doctor = doctor, ScheduledDate = date, Slot = slotname };
            Database.Appointments.Add(newappointment);
            return newappointment;
        }

        //CANCELLING APPOINTMENT
        public bool CancelAppointment(string appointmentid, string reason)
        {
            var appointment = Database.Appointments.FirstOrDefault(app => app.Appointment_id == appointmentid);
            if (appointment == null)
            {
                throw new AppointmentConflictException("Appointment id doesn't exist");
            }
            appointment.CancellationReason = reason;
            appointment.Status = Appointment.AppointmentStatus.Cancelled;
            return true;
        }

        //FETCHING APPOINTMENTS VIA PATIENT ID
        public List<Appointment> GetAppointmentsByPatient(int patientid)
        {
            List<Appointment> appointmentbypatientid = Database.Appointments.Where(app => app.Patient.PatientId == patientid).ToList();
            if (appointmentbypatientid.Count == 0)
            {
                throw new AppointmentConflictException("Appointment with this Patient ID not found");
            }
            return appointmentbypatientid;

        }

        //FETCHING APPOINTMENTS VIA DOCTOR ID
        public List<Appointment> GetAppointmentsByDoctor(int doctorid)
        {
            var appointmentbydoctorid = Database.Appointments.Where(app => app.Doctor.DoctorId == doctorid).ToList();
            if (appointmentbydoctorid.Count == 0)
            {
                throw new AppointmentConflictException("Appointment with this Doctor ID not found");
            }
            return appointmentbydoctorid;
        }

        //FETCHING APPOINTMENTS
        public List<Appointment> GetUpcomingAppointments()
        {
            if (Database.Appointments.Count == 0)
            {
                throw new AppointmentConflictException("No Appointments!");
            }
            return Database.Appointments;
        }

        //FETCHING APPOINTMENT VIA APPOINTMENT ID
        public Appointment? GetAppointmentById(string appointmentid)
        {
            var appointment = Database.Appointments.FirstOrDefault(app => app.Appointment_id == appointmentid);
            if (appointment == null)
            {
                throw new AppointmentConflictException("Appointment with this ID doesn't exist");
            }
            return appointment;
        }
    }
}
