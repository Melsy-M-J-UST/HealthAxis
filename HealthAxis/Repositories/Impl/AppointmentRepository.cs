using HealthAxis.Data;
using HealthAxis.Models;
using HealthAxis.Repositories;

namespace HealthAxis.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public Appointment Add(Appointment appointment)
        {
            appointment.AppointmentId = _context.GetNextAppointmentId();

            _context.Appointments.Add(appointment);
            appointment.Doctor.Appointments.Add(appointment);

            return appointment;
        }

        public Appointment? GetById(int appointmentId)
        {
            return _context.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
        }

        public List<Appointment> GetAll()
        {
            return _context.Appointments
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }

        public List<Appointment> GetByPatientId(int patientId)
        {
            return _context.Appointments
                .Where(a => a.Patient.PatientId == patientId)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }

        public List<Appointment> GetByDoctorId(int doctorId)
        {
            return _context.Appointments
                .Where(a => a.Doctor.DoctorId == doctorId)
                .OrderBy(a => a.ScheduledDate)
                .ThenBy(a => a.TimeSlot)
                .ToList();
        }
        public string? GetNextAvailableSlot(int doctorId, DateTime date)
        {
            var bookedSlots = _context.Appointments
                .Where(a =>
                    a.Doctor.DoctorId == doctorId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.Status != Appointment.StatusOption.Cancelled)
                .Select(a => a.TimeSlot)
                .ToList();

            foreach (var slot in _context.DailySlots)
            {
                bool isSlotBooked = bookedSlots.Any(bookedSlot =>
                    bookedSlot.Equals(slot, StringComparison.OrdinalIgnoreCase));

                if (!isSlotBooked)
                {
                    return slot;
                }
            }

            return null;
        }
        public int GetBookedSlotCount(int doctorId, DateTime date)
        {
            return _context.Appointments.Count(a =>
                a.Doctor.DoctorId == doctorId &&
                a.ScheduledDate.Date == date.Date &&
                a.Status != Appointment.StatusOption.Cancelled);
        }

        public void Remove(Appointment appointment)
        {
            if (appointment == null) return;

            _context.Appointments.Remove(appointment);
            if (appointment.Doctor != null)
            {
                appointment.Doctor.Appointments.RemoveAll(a => a.AppointmentId == appointment.AppointmentId);
            }
        }

        public bool PatientHasAppointmentAt(int patientId, DateTime date, string timeSlot)
        {
            return _context.Appointments.Any(a =>
                a.Patient.PatientId == patientId &&
                a.ScheduledDate.Date == date.Date &&
                string.Equals(a.TimeSlot, timeSlot, StringComparison.OrdinalIgnoreCase) &&
                a.Status != Appointment.StatusOption.Cancelled);
        }



        public string? GetNextAvailableSlotAvoidingPatientConflicts(int doctorId, DateTime date, int patientId)
        {
            var bookedSlotsForDoctor = _context.Appointments
                .Where(a =>
                    a.Doctor.DoctorId == doctorId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.Status != Appointment.StatusOption.Cancelled)
                .Select(a => a.TimeSlot)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var patientBookedSlots = _context.Appointments
                .Where(a =>
                    a.Patient.PatientId == patientId &&
                    a.ScheduledDate.Date == date.Date &&
                    a.Status != Appointment.StatusOption.Cancelled)
                .Select(a => a.TimeSlot)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var slot in _context.DailySlots)
            {
                if (bookedSlotsForDoctor.Contains(slot)) continue;
                if (patientBookedSlots.Contains(slot)) continue;
                return slot;
            }

            return null;
        }
    }
}