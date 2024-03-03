using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hackathon_template.Migrations
{
    /// <inheritdoc />
    public partial class Alerts2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserSettings_ShareLocation",
                table: "AspNetUsers",
                newName: "userSettings_ShareLocation");

            migrationBuilder.RenameColumn(
                name: "UserSettings_Longitude",
                table: "AspNetUsers",
                newName: "userSettings_Longitude");

            migrationBuilder.RenameColumn(
                name: "UserSettings_Latitude",
                table: "AspNetUsers",
                newName: "userSettings_Latitude");

            migrationBuilder.RenameColumn(
                name: "UserSettings_GetEmailAlerts",
                table: "AspNetUsers",
                newName: "userSettings_GetEmailAlerts");

            migrationBuilder.RenameColumn(
                name: "UserSettings_AlertsFromInDays",
                table: "AspNetUsers",
                newName: "userSettings_AlertsFromInDays");

            migrationBuilder.RenameColumn(
                name: "UserSettings_AlertRadius",
                table: "AspNetUsers",
                newName: "userSettings_AlertRadius");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userSettings_ShareLocation",
                table: "AspNetUsers",
                newName: "UserSettings_ShareLocation");

            migrationBuilder.RenameColumn(
                name: "userSettings_Longitude",
                table: "AspNetUsers",
                newName: "UserSettings_Longitude");

            migrationBuilder.RenameColumn(
                name: "userSettings_Latitude",
                table: "AspNetUsers",
                newName: "UserSettings_Latitude");

            migrationBuilder.RenameColumn(
                name: "userSettings_GetEmailAlerts",
                table: "AspNetUsers",
                newName: "UserSettings_GetEmailAlerts");

            migrationBuilder.RenameColumn(
                name: "userSettings_AlertsFromInDays",
                table: "AspNetUsers",
                newName: "UserSettings_AlertsFromInDays");

            migrationBuilder.RenameColumn(
                name: "userSettings_AlertRadius",
                table: "AspNetUsers",
                newName: "UserSettings_AlertRadius");
        }
    }
}
