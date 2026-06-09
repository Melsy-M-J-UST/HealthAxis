using HealthAxisWebApp;
using System.Collections.Generic;

namespace HealthAxis.Shared.Services.Interfaces
{
    public interface IDoctorService
    {
        List<Doctor> GetAllDoctors();
        Doctor GetDoctorById(int id);

        void AddDoctor(Doctor doctor);

        void UpdateDoctor(Doctor doctor);

        void DeleteDoctor(int id);
    }
}
