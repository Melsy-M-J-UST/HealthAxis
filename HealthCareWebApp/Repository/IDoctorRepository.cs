using HealthCareWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareWebApp.Repository
{
    public interface IDoctorRepository
    {
        Doctor AddDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        Doctor GetDoctorById(int doctorid);
        List<Doctor> SearchDoctorBySpecialisation(Doctor.Specialisations specialisation);
        bool UpdateDoctor(Doctor doctor);
    }
}
