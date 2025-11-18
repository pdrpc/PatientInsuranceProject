using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Configurations
{
    public class PatientConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patients");
            // Chave Primária
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Gender)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            // --- REQUISITO 14/28: CPF (válido e único) ---
            builder.Property(p => p.Cpf)
                .HasMaxLength(11); // Armazenar limpo (só números)

            // Cria um índice único no banco para o CPF. O filtro 'WHERE Cpf IS NOT NULL' permite múltiplos nulos
            builder.HasIndex(p => p.Cpf)
                .IsUnique()
                .HasFilter("[Cpf] IS NOT NULL");

            builder.Property(p => p.Email)
                .HasMaxLength(255);

            builder.Property(p => p.MobilePhone)
                .HasMaxLength(15);

            builder.Property(p => p.LandlinePhone)
                .HasMaxLength(15);

            // --- REQUISITO 26: Exclusão Lógica ---
            // Configura um "Query Filter" global. Isso garante que QUALQUER consulta do EF Core (GetAll, GetById) SEMPRE filtre por 'IsActive = true'
            builder.HasQueryFilter(p => p.IsActive);

            // Relacionamento com HealthInsurance
            builder.HasOne(p => p.HealthInsurance)
                .WithMany()
                .HasForeignKey(p => p.HealthInsuranceId)
                .IsRequired();
        }
    }
}
