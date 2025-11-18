using Application.DTOs.Patient;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Abstractions;
using Domain.Common;
using Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreatePatientDto> _createValidator;
        private readonly IValidator<UpdatePatientDto> _updateValidator;
        private readonly IUnitOfWork _unitOfWork;

        public PatientService(IPatientRepository patientRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<CreatePatientDto> createValidator,
            IValidator<UpdatePatientDto> updateValidator)
        {
            _patientRepository = patientRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto createDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var patient = _mapper.Map<Patient>(createDto);

            _patientRepository.Add(patient);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<bool> DeletePatientAsync(Guid id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null || !patient.IsActive)
            {
                return false;
            }

            patient.IsActive = false;
            _patientRepository.Update(patient);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<PaginatedResult<PatientDto>> GetPatientsAsync(int page, int pageSize, string? searchTerm)
        {
            var pagedData = await _patientRepository.GetPagedAsync(page, pageSize, searchTerm);

            var dtos = _mapper.Map<IEnumerable<PatientDto>>(pagedData.Items);

            return new PaginatedResult<PatientDto>(dtos, pagedData.TotalCount, page, pageSize);
        }

        public async Task<PatientDto?> GetPatientByIdAsync(Guid id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null || !patient.IsActive)
            {
                return null;
            }
            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<bool> UpdatePatientAsync(Guid id, UpdatePatientDto updateDto)
        {
            var validationContext = new ValidationContext<UpdatePatientDto>(updateDto);
            validationContext.RootContextData["PatientId"] = id;

            var validationResult = await _updateValidator.ValidateAsync(validationContext);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var patient = await _patientRepository.GetByIdAsync(id);
            if (patient == null || !patient.IsActive)
            {
                return false;
            }

            _mapper.Map(updateDto, patient);

            _patientRepository.Update(patient);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
