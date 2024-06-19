using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicRepositories.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAppointv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Clinic",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "ClinicID",
                table: "Appointment",
                newName: "ClinicId");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_ClinicID",
                table: "Appointment",
                newName: "IX_Appointment_ClinicId");

            migrationBuilder.AlterColumn<int>(
                name: "ClinicId",
                table: "Appointment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Clinic_ClinicId",
                table: "Appointment",
                column: "ClinicId",
                principalTable: "Clinic",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Clinic_ClinicId",
                table: "Appointment");

            migrationBuilder.RenameColumn(
                name: "ClinicId",
                table: "Appointment",
                newName: "ClinicID");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_ClinicId",
                table: "Appointment",
                newName: "IX_Appointment_ClinicID");

            migrationBuilder.AlterColumn<int>(
                name: "ClinicID",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Clinic",
                table: "Appointment",
                column: "ClinicID",
                principalTable: "Clinic",
                principalColumn: "ID");
        }
    }
}
