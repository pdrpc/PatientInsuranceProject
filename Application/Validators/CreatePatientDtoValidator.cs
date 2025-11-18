using Application.DTOs.Patient;
using Domain.Abstractions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
    {
        private readonly IPatientRepository _patientRepository;

        public CreatePatientDtoValidator(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;

            // --- Regras de Preenchimento Básico ---
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(100).WithMessage("First name must not exceed 100 characters.");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(100).WithMessage("Last name must not exceed 100 characters.");

            RuleFor(p => p.HealthInsuranceId)
                .NotEmpty().WithMessage("Health insurance is required.");

            RuleFor(p => p.Gender)
                .IsInEnum().WithMessage("Invalid gender.");

            // --- Regras de Negócio (Requisitos) ---
            RuleFor(p => p.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Today).WithMessage("Date of birth cannot be in the future.");

            RuleFor(p => p.Email)
                .EmailAddress().WithMessage("A valid email is required.")
                .When(p => !string.IsNullOrEmpty(p.Email));

            RuleFor(p => p)
                .Must(p => !string.IsNullOrEmpty(p.MobilePhone) || !string.IsNullOrEmpty(p.LandlinePhone))
                .WithMessage("At least one phone number (mobile or landline) must be provided.");

            RuleFor(p => p.Cpf)
                .Must(CustomValidationRules.BeAValidCpf).WithMessage("CPF must be valid.")
                .When(p => !string.IsNullOrEmpty(p.Cpf))
                .MustAsync(BeUniqueCpfAsync).WithMessage("This CPF is already in use.")
                .When(p => !string.IsNullOrEmpty(p.Cpf));
        }

        private async Task<bool> BeUniqueCpfAsync(string? cpf, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(cpf))
                return true;

            return !await _patientRepository.CpfExistsAsync(cpf);
        }
    }
}
