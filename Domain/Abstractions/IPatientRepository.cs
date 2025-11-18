using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface IPatientRepository
    {
        Task<Patient?> GetByIdAsync(Guid id);
        Task<PaginatedResult<Patient>> GetPagedAsync(int page, int pageSize, string? searchTerm);
        Task<bool> CpfExistsAsync(string cpf);
        Task<Patient?> GetByCpfAsync(string cpf);

        void Add(Patient patient);
        void Update(Patient patient);
    }
}
