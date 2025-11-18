using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HealthInsurances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HealthInsurances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    Rg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RgUf = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MobilePhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    LandlinePhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    HealthInsuranceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InsuranceCardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceCardExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_HealthInsurances_HealthInsuranceId",
                        column: x => x.HealthInsuranceId,
                        principalTable: "HealthInsurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "HealthInsurances",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("285aefa0-5a49-4ca3-947c-93ce37974906"), "Unimed" },
                    { new Guid("2dc276f9-3afc-4f24-940d-7a45ba0a1dda"), "Porto Seguro Saúde" },
                    { new Guid("40ed8af4-bea3-4718-8a74-af26b242bd42"), "Amil" },
                    { new Guid("8dd6a0c9-75af-487f-8b45-3a1363d9cd9f"), "SulAmérica" },
                    { new Guid("b3c3105e-356d-493a-b868-b1aded4b5896"), "Notredame Intermédica" },
                    { new Guid("c0f97a07-07d6-4007-a311-bc1eb8b451d6"), "Bradesco Saúde" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Cpf",
                table: "Patients",
                column: "Cpf",
                unique: true,
                filter: "[Cpf] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_HealthInsuranceId",
                table: "Patients",
                column: "HealthInsuranceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "HealthInsurances");
        }
    }
}
