using Application.DTOs.Patient;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IPatientService
    {
        Task<PaginatedResult<PatientDto>> GetPatientsAsync(int page, int pageSize, string? searchTerm);
        Task<PatientDto?> GetPatientByIdAsync(Guid id);
        Task<PatientDto> CreatePatientAsync(CreatePatientDto createDto);
        Task<bool> UpdatePatientAsync(Guid id, UpdatePatientDto updateDto);
        Task<bool> DeletePatientAsync(Guid id);
    }
}
