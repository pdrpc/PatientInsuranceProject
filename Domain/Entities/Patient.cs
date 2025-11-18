using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Patient
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; } 
        public EGender Gender { get; set; } 

        public string? Cpf { get; set; } 
        public string? Rg { get; set; } 
        public string? RgUf { get; set; } 

        public string? Email { get; set; }
        public string? MobilePhone { get; set; } 
        public string? LandlinePhone { get; set; } 

        public Guid HealthInsuranceId { get; set; } 
        public virtual HealthInsurance HealthInsurance { get; set; } = null!;

        public string? InsuranceCardNumber { get; set; } 
        public DateTime? InsuranceCardExpiryDate { get; set; } 

        public bool IsActive { get; set; } 

        public Patient()
        {
            Id = Guid.NewGuid();
            IsActive = true;
        }
    }
}
