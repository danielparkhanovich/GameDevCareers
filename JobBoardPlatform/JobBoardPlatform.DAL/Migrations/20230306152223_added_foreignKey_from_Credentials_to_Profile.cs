using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class added_foreignKey_from_Credentials_to_Profile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companies",
                table: "Companies");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "EmployeeCredentials");

            migrationBuilder.RenameTable(
                name: "Companies",
                newName: "CompanyCredentials");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_Email",
                table: "EmployeeCredentials",
                newName: "IX_EmployeeCredentials_Email");

            migrationBuilder.RenameIndex(
                name: "IX_Companies_Email",
                table: "CompanyCredentials",
                newName: "IX_CompanyCredentials_Email");

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "EmployeeCredentials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "CompanyCredentials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeCredentials",
                table: "EmployeeCredentials",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyCredentials",
                table: "CompanyCredentials",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCredentials_ProfileId",
                table: "EmployeeCredentials",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCredentials_ProfileId",
                table: "CompanyCredentials",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyCredentials_CompanyProfiles_ProfileId",
                table: "CompanyCredentials",
                column: "ProfileId",
                principalTable: "CompanyProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeCredentials_EmployeeProfiles_ProfileId",
                table: "EmployeeCredentials",
                column: "ProfileId",
                principalTable: "EmployeeProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyCredentials_CompanyProfiles_ProfileId",
                table: "CompanyCredentials");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeCredentials_EmployeeProfiles_ProfileId",
                table: "EmployeeCredentials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeCredentials",
                table: "EmployeeCredentials");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeCredentials_ProfileId",
                table: "EmployeeCredentials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyCredentials",
                table: "CompanyCredentials");

            migrationBuilder.DropIndex(
                name: "IX_CompanyCredentials_ProfileId",
                table: "CompanyCredentials");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "EmployeeCredentials");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "CompanyCredentials");

            migrationBuilder.RenameTable(
                name: "EmployeeCredentials",
                newName: "Employees");

            migrationBuilder.RenameTable(
                name: "CompanyCredentials",
                newName: "Companies");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeCredentials_Email",
                table: "Employees",
                newName: "IX_Employees_Email");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyCredentials_Email",
                table: "Companies",
                newName: "IX_Companies_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companies",
                table: "Companies",
                column: "Id");
        }
    }
}
