using HealthCareWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareWebApp.Service
{
    public interface IDoctorService
    {
        Doctor AddDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        Doctor GetDoctorById(int doctorId);
        List<Doctor> SearchDoctorBySpecialisation(Doctor.Specialisations specialisation);
        bool UpdateDoctor(Doctor doctor);
    }
}
