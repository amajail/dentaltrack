using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPerformanceIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Treatments_PatientId",
                table: "Treatments");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastLoginAt",
                table: "Users",
                column: "LastLoginAt");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_PatientId_Status",
                table: "Treatments",
                columns: new[] { "PatientId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_StartDate",
                table: "Treatments",
                column: "StartDate");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_IsProcessed",
                table: "Photos",
                column: "IsProcessed");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CreatedAt",
                table: "Patients",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_IsActive",
                table: "Patients",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Name",
                table: "Patients",
                columns: new[] { "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_CompletedAt",
                table: "Analyses",
                column: "CompletedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_LastLoginAt",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_PatientId_Status",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_StartDate",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Photos_IsProcessed",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Patients_CreatedAt",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_IsActive",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_Name",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Analyses_CompletedAt",
                table: "Analyses");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_PatientId",
                table: "Treatments",
                column: "PatientId");
        }
    }
}
