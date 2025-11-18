using Application.DTOs.Patient;
using Application.Services;
using AutoMapper;
using Domain.Abstractions;
using Domain.Common;
using Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace PatientInsuranceProject.UnitTests.Application.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _mockPatientRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IValidator<CreatePatientDto>> _mockCreateValidator;
        private readonly Mock<IValidator<UpdatePatientDto>> _mockUpdateValidator;

        private readonly PatientService _patientService;

        private readonly CreatePatientDto _validCreateDto;
        private readonly Patient _patient;
        private readonly PatientDto _patientDto;

        public PatientServiceTests()
        {
            _mockPatientRepo = new Mock<IPatientRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockCreateValidator = new Mock<IValidator<CreatePatientDto>>();
            _mockUpdateValidator = new Mock<IValidator<UpdatePatientDto>>();

            _patientService = new PatientService(
                _mockPatientRepo.Object,
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockCreateValidator.Object,
                _mockUpdateValidator.Object
            );

            var patientId = Guid.NewGuid();
            _validCreateDto = new CreatePatientDto { FirstName = "Test", LastName = "User" };
            _patient = new Patient { Id = patientId, FirstName = "Test", LastName = "User", IsActive = true };
            _patientDto = new PatientDto { Id = patientId, FirstName = "Test", LastName = "User" };
        }

        [Fact]
        public async Task CreatePatientAsync_WhenDtoIsValid_ShouldAddAndSaveChanges()
        {
            _mockCreateValidator
                .Setup(v => v.ValidateAsync(It.IsAny<CreatePatientDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mockMapper.Setup(m => m.Map<Patient>(_validCreateDto)).Returns(_patient);
            _mockMapper.Setup(m => m.Map<PatientDto>(_patient)).Returns(_patientDto);

            var result = await _patientService.CreatePatientAsync(_validCreateDto);

            Assert.NotNull(result);
            Assert.Equal(_patient.Id, result.Id);

            _mockPatientRepo.Verify(r => r.Add(_patient), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreatePatientAsync_WhenDtoIsInvalid_ShouldThrowValidationException()
        {
            var validationErrors = new List<ValidationFailure> { new ValidationFailure("FirstName", "Erro") };
            _mockCreateValidator
                .Setup(v => v.ValidateAsync(It.IsAny<CreatePatientDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(validationErrors));

            await Assert.ThrowsAsync<ValidationException>(() => _patientService.CreatePatientAsync(_validCreateDto));

            _mockPatientRepo.Verify(r => r.Add(It.IsAny<Patient>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenPatientExistsAndDtoIsValid_ShouldUpdateAndSaveChanges()
        {
            var updateDto = new UpdatePatientDto { FirstName = "Updated" };

            _mockUpdateValidator
                .Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<UpdatePatientDto>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mockPatientRepo.Setup(r => r.GetByIdAsync(_patient.Id)).ReturnsAsync(_patient);

            var result = await _patientService.UpdatePatientAsync(_patient.Id, updateDto);

            Assert.True(result);
            _mockPatientRepo.Verify(r => r.Update(_patient), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeletePatientAsync_WhenPatientExistsAndIsActive_ShouldSetInactiveAndUpdate()
        {
            _patient.IsActive = true;
            _mockPatientRepo.Setup(r => r.GetByIdAsync(_patient.Id)).ReturnsAsync(_patient);

            var result = await _patientService.DeletePatientAsync(_patient.Id);

            Assert.True(result);
            Assert.False(_patient.IsActive);
            _mockPatientRepo.Verify(r => r.Update(_patient), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetPatientByIdAsync_WhenPatientExistsAndIsActive_ShouldReturnDto()
        {
            _patient.IsActive = true;
            _mockPatientRepo.Setup(r => r.GetByIdAsync(_patient.Id)).ReturnsAsync(_patient);
            _mockMapper.Setup(m => m.Map<PatientDto>(_patient)).Returns(_patientDto);

            var result = await _patientService.GetPatientByIdAsync(_patient.Id);

            Assert.NotNull(result);
            Assert.Equal(_patientDto.Id, result.Id);
        }

        [Fact]
        public async Task GetPatientsAsync_ShouldReturnPaginatedResult()
        {
            // Arrange
            var activePatient = new Patient { Id = Guid.NewGuid(), FirstName = "Active", IsActive = true };
            var patientList = new List<Patient> { activePatient };
            var dtoList = new List<PatientDto> { new PatientDto { Id = activePatient.Id } };

            var pagedResult = new PaginatedResult<Patient>(patientList, 1, 1, 10);

            _mockPatientRepo
                .Setup(r => r.GetPagedAsync(1, 10, null))
                .ReturnsAsync(pagedResult);

            _mockMapper
                .Setup(m => m.Map<IEnumerable<PatientDto>>(patientList))
                .Returns(dtoList);

            // Act
            var result = await _patientService.GetPatientsAsync(1, 10, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.TotalCount);
            Assert.Equal(1, result.CurrentPage);
            Assert.Single(result.Items);
        }
    }
}