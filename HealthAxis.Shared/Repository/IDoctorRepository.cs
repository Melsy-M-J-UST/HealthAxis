using System.Collections.Generic;

namespace HealthAxisWebApp.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        List<Doctor> GetAll();

        Doctor GetById(int id);

        void Add(Doctor doctor);

        void Update(Doctor doctor);

        void Delete(int id);
    }

}
