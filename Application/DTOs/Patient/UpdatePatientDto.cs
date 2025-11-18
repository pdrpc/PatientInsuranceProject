using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Patient
{
    public class UpdatePatientDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public EGender Gender { get; set; }

        public string? Cpf { get; set; }
        public string? Rg { get; set; }
        public string? RgUf { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? MobilePhone { get; set; }
        public string? LandlinePhone { get; set; }

        [Required]
        public Guid HealthInsuranceId { get; set; }

        public string? InsuranceCardNumber { get; set; }
        public DateTime? InsuranceCardExpiryDate { get; set; }
    }
}
