using System.Collections.Generic;
using HealthAxis.Shared.Models;

namespace HealthAxis.Shared.Services.Interfaces
{
    public interface IDoctorService
    {
        List<Doctor> GetAllDoctors();
        List<Doctor> GetAllDoctors(string sortBy, string specialisation);
        Doctor GetDoctorById(int id);
        void AddDoctor(Doctor doctor);
        void UpdateDoctor(Doctor doctor);
        void DeleteDoctor(int id);
        int GetUpcomingAppointmentCount(int doctorId);
    }
}