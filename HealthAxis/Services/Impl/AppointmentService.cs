using HealthAxis.Exceptions;
using HealthAxis.Models;
using HealthAxis.Repositories;

namespace HealthAxis.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public Appointment BookAppointment(Patient patient, Doctor doctor, DateTime date)
        {
            if (patient == null)
            {
                throw new ArgumentException("Patient is required.");
            }

            if (doctor == null)
            {
                throw new ArgumentException("Doctor is required.");
            }

            if (date.Date < DateTime.Today)
            {
                throw new PastDateException("Cannot book an appointment in the past.");
            }
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new DoctorUnavailableException("Doctor is unavailable on sundays.");
            }

            if (!doctor.IsActive)
            {
                throw new DoctorUnavailableException("Doctor is not active.");
            }


            var hasConflict = _appointmentRepository.GetByPatientId(patient.PatientId)
                .Any(a => a.Doctor.DoctorId == doctor.DoctorId);

            if (hasConflict)
            {
                throw new AppointmentConflictException("Patient already has an appointment with this doctor on the selected date.");
            }

            var availableSlot = _appointmentRepository.GetNextAvailableSlotAvoidingPatientConflicts(doctor.DoctorId, date, patient.PatientId);

            if (availableSlot == null)
            {
                // fallback to any available slot for the doctor
                availableSlot = _appointmentRepository.GetNextAvailableSlot(doctor.DoctorId, date);
            }

            if (availableSlot == null)
            {
                throw new DoctorUnavailableException("No available slots for this doctor on the selected date.");
            }
            if (_appointmentRepository.PatientHasAppointmentAt(patient.PatientId, date, availableSlot))
            {
                throw new AppointmentConflictException("Patient already has an appointment at the selected date and time slot.");
            }

            var appointment = new Appointment
            {
                Patient = patient,
                Doctor = doctor,
                ScheduledDate = date.Date,
                Slot = availableSlot,
                Status = Appointment.AppointmentStatus.Confirmed
            };
            return _appointmentRepository.AddAppointment(appointment);
        }

        public bool CancelAppointment(int appointmentId, string reason)
        {
            var appointment = _appointmentRepository.GetAppointmentById(appointmentId);

            if (appointment == null)
            {
                return false;
            }

            appointment.Cancel(reason);
            _appointmentRepository.Remove(appointment);
            return true;
        }

        public List<Appointment> GetAppointmentsByPatient(int patientId)
        {
            return _appointmentRepository.GetByPatientId(patientId);
        }

        public List<Appointment> GetAppointmentsByDoctor(int doctorId)
        {
            return _appointmentRepository.GetByDoctorId(doctorId);
        }

        public List<Appointment> GetUpcomingAppointments()
        {
            return _appointmentRepository.GetAllAppointments()
                .Where(a =>
                    a.ScheduledDate.Date >= DateTime.Today &&
                    a.Status == Appointment.AppointmentStatus.Confirmed)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.Doctor.FullName)
                .ToList();
        }

        public Appointment? GetById(int appointmentId)
        {
            return _appointmentRepository.GetAppointmentById(appointmentId);
        }

        public List<Appointment> GetAll()
        {
            return _appointmentRepository.GetAllAppointments();
        }
    }
}