using System.Collections.Generic;

namespace HealthAxisWebApp.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        List<Patient> GetAll();

        Patient GetById(int id);

        void Add(Patient patient);

        void Update(Patient patient);

        void Delete(int id);
    }

}
