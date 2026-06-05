using AutoMapper;
using HealthAxis_Web.Models;
using HealthAxis_Web.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthAxis_Web.App_Start
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, DoctorDto>();
            CreateMap<DoctorDto, Doctor>();

        }
    }
}