using HealthAxis.Data;
using HealthAxis.Exceptions;
using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repository.Implementation
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly Database _Db;
        public AppointmentRepository(Database db)
        {
            _Db = db;
        }

        public Appointment BookAppointment(Appointment newAppointment)
        {
            _Db.Appointments.Add(newAppointment);
            return newAppointment;
        }


        public bool CancelAppointment(int appointmentid, string reason)
        {
            var appointment = _Db.Appointments.FirstOrDefault(app => app.AppointmentId == appointmentid);
            if (appointment == null)
            {
                throw new AppointmentNotFoundException($"Appointment with id {appointmentid} is not found.");
            }
            appointment.CancellationReason = reason;
            appointment.Status = Appointment.AppointmentStatus.Cancelled;
            return true;
        }


        public List<Appointment> GetAppointmentsByPatient(int patientid)
        {
            List<Appointment> appointmentbypatientid = _Db.Appointments.Where(app => app.Patient.PatientId == patientid).ToList();
            if (appointmentbypatientid.Count == 0)
            {
                throw new AppointmentNotFoundException($"There are no appointments for patient with id {patientid}.");
            }
            return appointmentbypatientid;

        }


        public List<Appointment> GetAppointmentsByDoctor(int doctorid)
        {
            var appointmentbydoctorid = _Db.Appointments.Where(app => app.Doctor.DoctorId == doctorid).ToList();
            if (appointmentbydoctorid.Count == 0)
            {
                throw new AppointmentNotFoundException($"There are no appointments for doctor with id {doctorid}.");
            }
            return appointmentbydoctorid;
        }


        public List<Appointment> GetAllAppointments()
        {
            if (_Db.Appointments.Count == 0)
            {
                throw new AppointmentNotFoundException("Currently there are no appointments booked.");
            }
            return _Db.Appointments;
        }


        public Appointment? GetAppointmentById(int appointmentid)
        {
            var appointment = _Db.Appointments.FirstOrDefault(app => app.AppointmentId == appointmentid);
            if (appointment == null)
            {
                throw new AppointmentNotFoundException($"Appointment with id {appointmentid} doesn't exist");
            }
            return appointment;
        }
    }
}
