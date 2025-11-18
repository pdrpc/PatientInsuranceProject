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
    public class UpdatePatientDtoValidator : AbstractValidator<UpdatePatientDto>
    {
        private readonly IPatientRepository _patientRepository;

        public UpdatePatientDtoValidator(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;

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
                .MustAsync(BeUniqueOrSamePatientCpfAsync).WithMessage("This CPF is already in use by another patient.")
                .When(p => !string.IsNullOrEmpty(p.Cpf));

            // A validação de "CPF Único" para Update é mais complexa:
            // "O CPF deve ser único, A MENOS que seja o CPF do PRÓPRIO paciente"
            // Isso também exigirá uma chamada ao repositório.
        }

        private async Task<bool> BeUniqueOrSamePatientCpfAsync(UpdatePatientDto dto, string? cpf, ValidationContext<UpdatePatientDto> context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(cpf))
                return true;

            // Tenta obter o ID do paciente do contexto da validação
            if (!context.RootContextData.TryGetValue("PatientId", out var patientIdObj) || !(patientIdObj is Guid patientId))
            {
                // Se não conseguirmos o ID, não podemos validar com segurança
                return false;
            }

            var patientWithCpf = await _patientRepository.GetByCpfAsync(cpf);

            if (patientWithCpf == null)
                return true;

            // Não é único, mas é o CPF do próprio paciente que estamos editando?
            return patientWithCpf.Id == patientId;
        }

    }
}
