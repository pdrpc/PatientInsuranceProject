using Domain.Abstractions;
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
    public class HealthInsuranceRepository : IHealthInsuranceRepository
    {
        private readonly AppDbContext _context;

        public HealthInsuranceRepository( AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HealthInsurance>> GetAllAsync()
        {
            return await _context.HealthInsurances.AsNoTracking().ToListAsync();
        }
    }
}
