using System;
using System.Collections.Generic;
using System.Text;
using HAP_Pod4_ConsoleApp_au.Models;

namespace HAP_Pod4_ConsoleApp_au.Services
{
    public interface IDoctorService
    {
        Doctor AddDoctor(Doctor doctor);

        List<Doctor> GetAllDoctors();

        List<Doctor> SearchDoctorBySpecialisation(
            Doctor.SpecialisationOption specialisation);
    }
}

