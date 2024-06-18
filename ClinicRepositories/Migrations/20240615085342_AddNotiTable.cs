using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClinicRepositories.Migrations
{
    /// <inheritdoc />
    public partial class AddNotiTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Notification_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserID",
                table: "Notification",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification");
        }
    }
}
