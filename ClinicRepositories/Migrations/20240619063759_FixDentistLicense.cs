using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicRepositories.Migrations
{
    /// <inheritdoc />
    public partial class FixDentistLicense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dentist_License",
                table: "Dentist");

            migrationBuilder.DropIndex(
                name: "IX_Dentist_LicenseId",
                table: "Dentist");

            migrationBuilder.DropColumn(
                name: "LicenseId",
                table: "Dentist");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Dentist");

            migrationBuilder.AddColumn<int>(
                name: "DentistId",
                table: "License",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_License_DentistId",
                table: "License",
                column: "DentistId");

            migrationBuilder.AddForeignKey(
                name: "FK_License_Dentist",
                table: "License",
                column: "DentistId",
                principalTable: "Dentist",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_License_Dentist",
                table: "License");

            migrationBuilder.DropIndex(
                name: "IX_License_DentistId",
                table: "License");

            migrationBuilder.DropColumn(
                name: "DentistId",
                table: "License");

            migrationBuilder.AddColumn<int>(
                name: "LicenseId",
                table: "Dentist",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Dentist",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dentist_LicenseId",
                table: "Dentist",
                column: "LicenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dentist_License",
                table: "Dentist",
                column: "LicenseId",
                principalTable: "License",
                principalColumn: "ID");
        }
    }
}
