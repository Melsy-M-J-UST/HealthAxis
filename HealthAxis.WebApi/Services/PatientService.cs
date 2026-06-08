using HealthAxis.Api.Data;
using HealthAxis.Api.Repositories.Interfaces;
using HealthAxis.Api.Services.Interfaces;
using HealthAxis.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthAxis.Api.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public IEnumerable<PatientDto> GetAll(string insuranceStatus = null)
        {
            return _patientRepository
                .GetAll(insuranceStatus)
                .Select(patient => MapToDto(patient));
        }

        public PatientDto GetById(int id)
        {
            var patient = _patientRepository.GetById(id);

            if (patient == null)
            {
                return null;
            }

            var dto = MapToDto(patient);

            dto.AppointmentCount = _patientRepository.GetAppointmentCount(id);

            return dto;
        }

        public bool Create(PatientDto dto, out string errorMessage)
        {
            errorMessage = string.Empty;

            var existingPatient = _patientRepository.GetByEmail(dto.Email);

            if (existingPatient != null)
            {
                errorMessage = "A patient with this email already exists.";
                return false;
            }

            var patient = new Patient
            {
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                InsuranceID = dto.InsuranceID,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            _patientRepository.Add(patient);

            return true;
        }

        public bool Update(int id, PatientDto dto, out string errorMessage)
        {
            errorMessage = string.Empty;

            var duplicatePatient = _patientRepository.GetByEmail(dto.Email);

            if (duplicatePatient != null && duplicatePatient.PatientId != id)
            {
                errorMessage = "Another patient with this email already exists.";
                return false;
            }

            var patient = new Patient
            {
                PatientId = id,
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                InsuranceID = dto.InsuranceID
            };

            bool updated = _patientRepository.Update(patient);

            if (!updated)
            {
                errorMessage = "Patient not found.";
                return false;
            }

            return true;
        }

        public bool Deactivate(int id)
        {
            return _patientRepository.Deactivate(id);
        }

        private PatientDto MapToDto(Patient patient)
        {
            return new PatientDto
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email,
                InsuranceID = patient.InsuranceID,
                CreatedDate = patient.CreatedDate,
                IsActive = patient.IsActive
            };
        }
    }
}