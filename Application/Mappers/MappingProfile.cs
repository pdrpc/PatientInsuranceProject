using Application.DTOs.HealthInsurance;
using Application.DTOs.Patient;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<HealthInsurance, HealthInsuranceDto>();
            CreateMap<Patient, PatientDto>();
            CreateMap<CreatePatientDto, Patient>();
            CreateMap<UpdatePatientDto, Patient>();
        }
    }
}
