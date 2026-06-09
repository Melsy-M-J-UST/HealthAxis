using HealthAxis.Api.Data;
using HealthAxis.Api.Helpers;
using HealthAxis.Api.Repositories.Interfaces;
using HealthAxis.Api.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Enums;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Api.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public IEnumerable<DoctorDto> GetAll(
            string specialisation = null,
            bool activeOnly = false)
        {
            return _doctorRepository
                .GetAll(specialisation, activeOnly)
                .Select(Map);
        }

        public DoctorDto GetById(int id)
        {
            var doctor = _doctorRepository.GetById(id);

            if (doctor == null)
            {
                return null;
            }

            var dto = Map(doctor);

            dto.UpcomingAppointmentCount =
                _doctorRepository.GetUpcomingAppointmentCount(id);

            return dto;
        }

        public bool Create(DoctorDto dto, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (dto.ConsultationFee < 0)
            {
                errorMessage = "Fee cannot be negative.";
                return false;
            }

            var doctor = new Doctor
            {
                FullName = dto.FullName,
                Specialisation = dto.Specialisation.ToString(),
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = true
            };

            _doctorRepository.Add(doctor);

            return true;
        }

        public bool Update(
            int id,
            DoctorDto dto,
            out string errorMessage)
        {
            errorMessage = string.Empty;

            if (dto.ConsultationFee < 0)
            {
                errorMessage = "Fee cannot be negative.";
                return false;
            }

            var doctor = new Doctor
            {
                DoctorId = id,
                FullName = dto.FullName,
                Specialisation = dto.Specialisation.ToString(),
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = dto.IsActive
            };

            bool updated = _doctorRepository.Update(doctor);

            if (!updated)
            {
                errorMessage = "Doctor not found.";
                return false;
            }

            return true;
        }

        public bool ToggleStatus(int id)
        {
            return _doctorRepository.ToggleStatus(id);
        }

        private DoctorDto Map(Doctor doctor)
        {
            return new DoctorDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                Specialisation =
                    EnumMapper.ParseEnum<SpecialisationEnum>(
                        doctor.Specialisation),
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = doctor.IsActive
            };
        }
    }
}