using Application.DTOs.HealthInsurance;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class HealthInsuranceService : IHealthInsuranceService
    {
        private readonly IHealthInsuranceRepository _repository;
        private readonly IMapper _mapper;

        public HealthInsuranceService(IHealthInsuranceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<HealthInsuranceDto>> GetAllAsync()
        {
            var insurances = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<HealthInsuranceDto>>(insurances);
        }
    }
}
