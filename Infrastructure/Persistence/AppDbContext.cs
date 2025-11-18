using Domain.Abstractions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext, IUnitOfWork
    {


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        // "OnModelCreating" é onde configuramos o mapeamento (Code-First)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Esta linha varre todo o assembly (projeto Infrastructure) e aplica automaticamente todas as classes que implementam IEntityTypeConfiguration (nossas duas classes de config).
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<HealthInsurance> HealthInsurances { get; set; }
    }
}
