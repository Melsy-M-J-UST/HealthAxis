using System;
using System.Collections.Generic;
using System.Text;
using HealthAxis.Models;

namespace HealthAxis.Service
{
    public interface IDoctorService
    {
        Doctor AddDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation);
    }
}
