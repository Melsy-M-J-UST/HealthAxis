using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repositories
{
    public interface IDoctorRepository
    {
        Doctor AddDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
        Doctor? GetById(int doctorId);

        List<Doctor> SearchDoctorBySpecialisation(Doctor.SpecialisationOption specialisation);
    }
}
