using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicRepositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppointv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Clinic_ClinicId",
                table: "Appointment");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_ClinicId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "Appointment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClinicId",
                table: "Appointment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_ClinicId",
                table: "Appointment",
                column: "ClinicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Clinic_ClinicId",
                table: "Appointment",
                column: "ClinicId",
                principalTable: "Clinic",
                principalColumn: "ID");
        }
    }
}
