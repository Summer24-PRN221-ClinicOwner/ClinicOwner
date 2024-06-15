using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicRepositories.Migrations
{
    /// <inheritdoc />
    public partial class AddsenderinMessageTable4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "Message",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sender",
                table: "Message");
        }
    }
}
