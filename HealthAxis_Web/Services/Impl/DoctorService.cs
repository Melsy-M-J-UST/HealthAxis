using HealthAxis_Web.Models.Dtos;
using HealthAxis_MVC.Repositories;
using System.Collections.Generic;
using System;
using AutoMapper;
using HealthAxis_Web.Models;

namespace HealthAxis_MVC.Services.Impl
{
    public class DoctorServiceImpl : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;

        public DoctorServiceImpl(IDoctorRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public DoctorDto AddDoctor(DoctorDto entity)
        {
            var doctor = _mapper.Map<Doctor>(entity);
            var savedEntity = _repository.AddDoctor(doctor);
            var savedDto = _mapper.Map<DoctorDto>(savedEntity);
            return savedDto;

        }

        public List<DoctorDto> GetAllDoctors()
        {
            return _mapper.Map < List<DoctorDto>>(_repository.GetAllDoctors());
        }

        public DoctorDto GetById(int doctorId)
        {
            return _mapper.Map<DoctorDto>(_repository.GetById(doctorId));
        }

        DoctorDto IDoctorService.UpdateDoctor(int id,DoctorDto entity)
        {
            var doctor = _mapper.Map<Doctor>(entity);
            var updatedDoctor = _repository.UpdateDoctor(id, doctor);
            if (updatedDoctor == null)
            {
                return null;
            }
            return _mapper.Map<DoctorDto>(updatedDoctor);

        }
    }
}