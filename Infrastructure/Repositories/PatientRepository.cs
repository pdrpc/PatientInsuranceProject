using Domain.Abstractions;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Patient patient)
        {
            _context.Patients.Add(patient);
        }

        public async Task<bool> CpfExistsAsync(string cpf)
        {
            return await _context.Patients.IgnoreQueryFilters().AnyAsync(p => p.Cpf == cpf);
        }

        public async Task<PaginatedResult<Patient>> GetPagedAsync(int page, int pageSize, string? searchTerm)
        {
            var query = _context.Patients
                .AsNoTracking()
                .Include(p => p.HealthInsurance)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p =>
                    p.FirstName.Contains(searchTerm) ||
                    p.LastName.Contains(searchTerm) ||
                    p.Cpf!.Contains(searchTerm) ||
                    p.Email!.Contains(searchTerm) ||
                    p.HealthInsurance.Name.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Patient>(items, totalCount, page, pageSize);
        }

        public async Task<Patient?> GetByCpfAsync(string cpf)
        {
            return await _context.Patients.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Cpf == cpf);
        }

        public async Task<Patient?> GetByIdAsync(Guid id)
        {
            return await _context.Patients.Include(p => p.HealthInsurance).FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Update(Patient patient)
        {
            _context.Patients.Update(patient);
        }
    }
}
