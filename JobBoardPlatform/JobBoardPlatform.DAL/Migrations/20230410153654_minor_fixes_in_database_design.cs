using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class minor_fixes_in_database_design : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOfferEmploymentDetails_CurrencyTypes_SalaryCurrencyId",
                table: "JobOfferEmploymentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOfferEmploymentDetails_EmploymentTypes_EmploymentTypesId",
                table: "JobOfferEmploymentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOfferEmploymentDetails_JobOfferSalariesRange_SalaryFromRangeId",
                table: "JobOfferEmploymentDetails");

            migrationBuilder.DropIndex(
                name: "IX_JobOfferEmploymentDetails_EmploymentTypesId",
                table: "JobOfferEmploymentDetails");

            migrationBuilder.DropColumn(
                name: "EmploymentTypesId",
                table: "JobOfferEmploymentDetails");

            migrationBuilder.RenameColumn(
                name: "Published",
                table: "JobOffers",
                newName: "PublishedAt");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "JobOffers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "SalaryFromRangeId",
                table: "JobOfferEmploymentDetails",
                newName: "SalaryRangeId");

            migrationBuilder.RenameColumn(
                name: "SalaryCurrencyId",
                table: "JobOfferEmploymentDetails",
                newName: "EmploymentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_JobOfferEmploymentDetails_SalaryFromRangeId",
                table: "JobOfferEmploymentDetails",
                newName: "IX_JobOfferEmploymentDetails_SalaryRangeId");

            migrationBuilder.RenameIndex(
                name: "IX_JobOfferEmploymentDetails_SalaryCurrencyId",
                table: "JobOfferEmploymentDetails",
                newName: "IX_JobOfferEmploymentDetails_EmploymentTypeId");

            migrationBuilder.AddColumn<int>(
                name: "SalaryCurrencyId",
                table: "JobOfferSalariesRange",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "JobOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TechKeyWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobOfferId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechKeyWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechKeyWords_JobOffers_JobOfferId",
                        column: x => x.JobOfferId,
                        principalTable: "JobOffers",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "EmploymentTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "Permanent");

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferSalariesRange_SalaryCurrencyId",
                table: "JobOfferSalariesRange",
                column: "SalaryCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_TechKeyWords_JobOfferId",
                table: "TechKeyWords",
                column: "JobOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOfferEmploymentDetails_EmploymentTypes_EmploymentTypeId",
                table: "JobOfferEmploymentDetails",
                column: "EmploymentTypeId",
                principalTable: "EmploymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOfferEmploymentDetails_JobOfferSalariesRange_SalaryRangeId",
                table: "JobOfferEmploymentDetails",
                column: "SalaryRangeId",
                principalTable: "JobOfferSalariesRange",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOfferSalariesRange_CurrencyTypes_SalaryCurrencyId",
                table: "JobOfferSalariesRange",
                column: "SalaryCurrencyId",
                principalTable: "CurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOfferEmploymentDetails_EmploymentTypes_EmploymentTypeId",
                table: "JobOfferEmploymentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOfferEmploymentDetails_JobOfferSalariesRange_SalaryRangeId",
                table: "JobOfferEmploymentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOfferSalariesRange_CurrencyTypes_SalaryCurrencyId",
                table: "JobOfferSalariesRange");

            migrationBuilder.DropTable(
                name: "TechKeyWords");

            migrationBuilder.DropIndex(
                name: "IX_JobOfferSalariesRange_SalaryCurrencyId",
                table: "JobOfferSalariesRange");

            migrationBuilder.DropColumn(
                name: "SalaryCurrencyId",
                table: "JobOfferSalariesRange");

            migrationBuilder.DropColumn(
                name: "City",
                table: "JobOffers");

            migrationBuilder.RenameColumn(
                name: "PublishedAt",
                table: "JobOffers",
                newName: "Published");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "JobOffers",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "SalaryRangeId",
                table: "JobOfferEmploymentDetails",
                newName: "SalaryFromRangeId");

            migrationBuilder.RenameColumn(
                name: "EmploymentTypeId",
                table: "JobOfferEmploymentDetails",
                newName: "SalaryCurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_JobOfferEmploymentDetails_SalaryRangeId",
                table: "JobOfferEmploymentDetails",
                newName: "IX_JobOfferEmploymentDetails_SalaryFromRangeId");

            migrationBuilder.RenameIndex(
                name: "IX_JobOfferEmploymentDetails_EmploymentTypeId",
                table: "JobOfferEmploymentDetails",
                newName: "IX_JobOfferEmploymentDetails_SalaryCurrencyId");

            migrationBuilder.AddColumn<int>(
                name: "EmploymentTypesId",
                table: "JobOfferEmploymentDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "EmploymentTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Type",
                value: "ContractOfEmployment");

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferEmploymentDetails_EmploymentTypesId",
                table: "JobOfferEmploymentDetails",
                column: "EmploymentTypesId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOfferEmploymentDetails_CurrencyTypes_SalaryCurrencyId",
                table: "JobOfferEmploymentDetails",
                column: "SalaryCurrencyId",
                principalTable: "CurrencyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOfferEmploymentDetails_EmploymentTypes_EmploymentTypesId",
                table: "JobOfferEmploymentDetails",
                column: "EmploymentTypesId",
                principalTable: "EmploymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOfferEmploymentDetails_JobOfferSalariesRange_SalaryFromRangeId",
                table: "JobOfferEmploymentDetails",
                column: "SalaryFromRangeId",
                principalTable: "JobOfferSalariesRange",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
