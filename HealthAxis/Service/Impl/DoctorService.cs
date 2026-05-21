using HealthAxis.Models;
using HealthAxis.Repositories;
using HealthAxis.Service;

namespace HealthAxis.Service.Impl
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;

        public DoctorService(IDoctorRepository _repository)
        {
            this._repository = _repository;
        }
        public Doctor AddDoctor(Doctor doctor)
        {
            return _repository.AddDoctor(doctor);
        }

        public List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation)
        {
            {
                return _repository.SearchDoctorBySpecialisation(specialisation);
            }
        }
        public List<Doctor> GetAllDoctors()
        {
            return _repository.GetAllDoctors();
        }

    }
}
