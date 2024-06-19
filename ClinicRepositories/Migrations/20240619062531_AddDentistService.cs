using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicRepositories.Migrations
{
    /// <inheritdoc />
    public partial class AddDentistService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rank",
                table: "License");

            migrationBuilder.CreateTable(
                name: "DentistService",
                columns: table => new
                {
                    DentistId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DentistService", x => new { x.DentistId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_DentistService_Dentist",
                        column: x => x.DentistId,
                        principalTable: "Dentist",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_DentistService_Service",
                        column: x => x.ServiceId,
                        principalTable: "Service",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DentistService_ServiceId",
                table: "DentistService",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DentistService");

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "License",
                type: "int",
                nullable: true);
        }
    }
}
