using AutoMapper;
using HealthAxis.Shared.DTOs;
using HealthAxis.Shared.Models;
using HealthAxisWebApp;

namespace HealthAxis.Shared.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Patient Mapping rules
            CreateMap<Patient, PatientDto>();
            CreateMap<PatientDto, Patient>();

            // Doctor Mapping rules
            CreateMap<Doctor, DoctorDto>();
            CreateMap<DoctorDto, Doctor>();

            // Appointment Mapping rules
            CreateMap<Appointment, AppointmentDto>();
            CreateMap<AppointmentDto, Appointment>();

            // Health Record Mapping rules
            CreateMap<HealthRecord, HealthRecordDto>();
            CreateMap<HealthRecordDto, HealthRecord>();
        }
    }
}