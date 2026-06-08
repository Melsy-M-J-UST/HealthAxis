using HealthAxis.Api.Data;
using System.Collections.Generic;
using System.Numerics;

namespace HealthAxis.Api.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        IEnumerable<Doctor> GetAll();
        IEnumerable<Doctor> GetActiveDoctors();
        IEnumerable<Doctor> GetBySpecialisation(string specialisation);
        Doctor GetById(int id);
        Doctor Add(Doctor doctor);
        bool Update(Doctor doctor);
        bool ToggleStatus(int id);
    }
}