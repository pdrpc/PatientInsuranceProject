using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace PatientInsuranceProject.UnitTests.Infrastructure.Repositories
{
    public class PatientRepositoryTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new AppDbContext(options);
            return dbContext;
        }

        private async Task SeedData(AppDbContext context)
        {
            var healthInsurance = new HealthInsurance { Id = Guid.NewGuid(), Name = "Test HI" };
            context.HealthInsurances.Add(healthInsurance);

            context.Patients.AddRange(
                new Patient
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    FirstName = "Active",
                    LastName = "User",
                    IsActive = true,
                    Cpf = "11122233344",
                    HealthInsuranceId = healthInsurance.Id
                },
                new Patient
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    FirstName = "Inactive",
                    LastName = "User",
                    IsActive = false,
                    Cpf = "55566677788",
                    HealthInsuranceId = healthInsurance.Id
                }
            );
            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientIsActive_ShouldReturnPatient()
        {
            await using var context = GetInMemoryDbContext();
            await SeedData(context);
            var repository = new PatientRepository(context);
            var activePatientId = Guid.Parse("11111111-1111-1111-1111-111111111111");

            var patient = await repository.GetByIdAsync(activePatientId);

            Assert.NotNull(patient);
            Assert.Equal("Active", patient.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientIsInactive_ShouldReturnNull()
        {
            await using var context = GetInMemoryDbContext();
            await SeedData(context);
            var repository = new PatientRepository(context);
            var inactivePatientId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            var patient = await repository.GetByIdAsync(inactivePatientId);

            Assert.Null(patient);
        }

        // --- CORREÇÃO AQUI ---
        [Fact]
        public async Task GetPagedAsync_ShouldReturnOnlyActivePatientsAndPaginate()
        {
            await using var context = GetInMemoryDbContext();
            await SeedData(context);
            var repository = new PatientRepository(context);

            // Act: Chama o novo método com paginação
            var result = await repository.GetPagedAsync(1, 10, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.TotalCount); // Deve contar apenas 1 (o ativo)
            Assert.Single(result.Items); // A lista deve ter 1 item
            Assert.Equal("Active", result.Items.First().FirstName);
        }

        [Fact]
        public async Task CpfExistsAsync_WhenCpfBelongsToInactivePatient_ShouldReturnTrue()
        {
            await using var context = GetInMemoryDbContext();
            await SeedData(context);
            var repository = new PatientRepository(context);
            var inactiveCpf = "55566677788";

            var exists = await repository.CpfExistsAsync(inactiveCpf);

            Assert.True(exists);
        }

        [Fact]
        public async Task CpfExistsAsync_WhenCpfDoesNotExist_ShouldReturnFalse()
        {
            await using var context = GetInMemoryDbContext();
            await SeedData(context);
            var repository = new PatientRepository(context);

            var exists = await repository.CpfExistsAsync("00000000000");

            Assert.False(exists);
        }
    }
}