using HealthAxis.Shared.DTOs;
using System.Collections.Generic;

namespace HealthAxis.Mvc.Services.Interfaces
{
    public interface IDoctorMvcService
    {
        IEnumerable<DoctorDto> GetAll(
            string specialisation = null,
            bool activeOnly = false);

        DoctorDto GetById(int id);

        bool Create(
            DoctorDto dto,
            out string error);

        bool Update(
            DoctorDto dto,
            out string error);

        bool ToggleStatus(
            int id,
            out string error);
    }
}