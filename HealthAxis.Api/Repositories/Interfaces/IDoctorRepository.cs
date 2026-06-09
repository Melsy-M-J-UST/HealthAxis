using HealthAxis.Api.Data;
using System.Collections.Generic;

namespace HealthAxis.Api.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        IEnumerable<Doctor> GetAll(
            string specialisation = null,
            bool activeOnly = false);

        Doctor GetById(int id);

        Doctor Add(Doctor doctor);

        bool Update(Doctor doctor);

        bool ToggleStatus(int id);

        int GetUpcomingAppointmentCount(int doctorId);
    }
}