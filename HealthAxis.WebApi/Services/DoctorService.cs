using HealthAxis.Api.Data;
using HealthAxis.Api.Repositories.Interfaces;
using HealthAxis.Api.Services.Interfaces;
using HealthAxis.Shared.DTOs;
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

        public IEnumerable<DoctorDto> GetAll(string specialisation = null, bool activeOnly = false)
        {
            var doctors = activeOnly
                ? _doctorRepository.GetActiveDoctors()
                : string.IsNullOrWhiteSpace(specialisation)
                    ? _doctorRepository.GetAll()
                    : _doctorRepository.GetBySpecialisation(specialisation);

            return doctors.Select(MapToDto);
        }

        public DoctorDto GetById(int id)
        {
            var doctor = _doctorRepository.GetById(id);

            if (doctor == null)
            {
                return null;
            }

            return MapToDto(doctor);
        }

        public DoctorDto Create(DoctorDto dto)
        {
            var doctor = new Doctor
            {
                FullName = dto.FullName,
                Specialisation = dto.Specialisation,
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = true
            };

            var createdDoctor = _doctorRepository.Add(doctor);

            return MapToDto(createdDoctor);
        }

        public bool Update(int id, DoctorDto dto)
        {
            dto.DoctorId = id;

            var doctor = new Doctor
            {
                DoctorId = id,
                FullName = dto.FullName,
                Specialisation = dto.Specialisation,
                YearsOfExperience = dto.YearsOfExperience,
                ConsultationFee = dto.ConsultationFee,
                IsActive = dto.IsActive
            };

            return _doctorRepository.Update(doctor);
        }

        public bool ToggleStatus(int id)
        {
            return _doctorRepository.ToggleStatus(id);
        }

        private DoctorDto MapToDto(Doctor doctor)
        {
            return new DoctorDto
            {
                DoctorId = doctor.DoctorId,
                FullName = doctor.FullName,
                Specialisation = doctor.Specialisation,
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = doctor.IsActive
            };
        }
    }
}