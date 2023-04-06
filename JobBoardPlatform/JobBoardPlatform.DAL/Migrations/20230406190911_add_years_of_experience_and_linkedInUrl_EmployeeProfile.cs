using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class add_years_of_experience_and_linkedInUrl_EmployeeProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LinkedInUrl",
                table: "EmployeeProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YearsOfExperience",
                table: "EmployeeProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkedInUrl",
                table: "EmployeeProfiles");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "EmployeeProfiles");
        }
    }
}
