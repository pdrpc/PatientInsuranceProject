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
    public class HealthInsuranceConfiguration : IEntityTypeConfiguration<HealthInsurance>
    {
        public void Configure(EntityTypeBuilder<HealthInsurance> builder)
        {
            builder.HasKey(hi => hi.Id);
            builder.Property(hi => hi.Name)
                .IsRequired()
                .HasMaxLength(150);
            builder.ToTable("HealthInsurances");

            // --- REQUISITO 41: Dados mock para a tabela de convênios ---
            // Vamos adicionar os dados mock aqui. O EF Core usará "Migrations" para inserir esses dados no banco (seeding).
            builder.HasData(
                new HealthInsurance { Id = Guid.Parse("285aefa0-5a49-4ca3-947c-93ce37974906"), Name = "Unimed" },
                new HealthInsurance { Id = Guid.Parse("2dc276f9-3afc-4f24-940d-7a45ba0a1dda"), Name = "Porto Seguro Saúde" },
                new HealthInsurance { Id = Guid.Parse("40ed8af4-bea3-4718-8a74-af26b242bd42"), Name = "Amil" },
                new HealthInsurance { Id = Guid.Parse("8dd6a0c9-75af-487f-8b45-3a1363d9cd9f"), Name = "SulAmérica" },
                new HealthInsurance { Id = Guid.Parse("b3c3105e-356d-493a-b868-b1aded4b5896"), Name = "Notredame Intermédica" },
                new HealthInsurance { Id = Guid.Parse("c0f97a07-07d6-4007-a311-bc1eb8b451d6"), Name = "Bradesco Saúde" }
            );
        }
    }
}
