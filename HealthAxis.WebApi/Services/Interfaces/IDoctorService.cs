using HealthAxis.Shared.DTOs;
using System.Collections.Generic;

namespace HealthAxis.Api.Services.Interfaces
{
    public interface IDoctorService
    {
        IEnumerable<DoctorDto> GetAll(string specialisation = null, bool activeOnly = false);
        DoctorDto GetById(int id);
        DoctorDto Create(DoctorDto dto);
        bool Update(int id, DoctorDto dto);
        bool ToggleStatus(int id);
    }
}
