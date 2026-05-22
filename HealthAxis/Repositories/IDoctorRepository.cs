using HealthAxis.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthAxis.Repositories
{
    public class IDoctorRepository
    {
        string AddDoctor(Doctor doctor);

        List<Doctor> SearchDoctorBySpecialisation(Doctor doctor);
    }
}
