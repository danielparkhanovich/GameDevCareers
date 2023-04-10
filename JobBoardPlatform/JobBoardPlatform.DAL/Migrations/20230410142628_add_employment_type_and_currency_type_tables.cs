using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class add_employment_type_and_currency_type_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmploymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkLocationType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOffers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobOfferSalariesRange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    From = table.Column<int>(type: "int", nullable: false),
                    To = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOfferSalariesRange", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobOfferEmploymentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmploymentTypesId = table.Column<int>(type: "int", nullable: false),
                    SalaryFromRangeId = table.Column<int>(type: "int", nullable: false),
                    SalaryCurrencyId = table.Column<int>(type: "int", nullable: false),
                    JobOfferId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOfferEmploymentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobOfferEmploymentDetails_CurrencyTypes_SalaryCurrencyId",
                        column: x => x.SalaryCurrencyId,
                        principalTable: "CurrencyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobOfferEmploymentDetails_EmploymentTypes_EmploymentTypesId",
                        column: x => x.EmploymentTypesId,
                        principalTable: "EmploymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobOfferEmploymentDetails_JobOfferSalariesRange_SalaryFromRangeId",
                        column: x => x.SalaryFromRangeId,
                        principalTable: "JobOfferSalariesRange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobOfferEmploymentDetails_JobOffers_JobOfferId",
                        column: x => x.JobOfferId,
                        principalTable: "JobOffers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "CurrencyTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "PLN" },
                    { 2, "EUR" }
                });

            migrationBuilder.InsertData(
                table: "EmploymentTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "ContractOfEmployment" },
                    { 2, "B2B" },
                    { 3, "MandatoryContract" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferEmploymentDetails_EmploymentTypesId",
                table: "JobOfferEmploymentDetails",
                column: "EmploymentTypesId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferEmploymentDetails_JobOfferId",
                table: "JobOfferEmploymentDetails",
                column: "JobOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferEmploymentDetails_SalaryCurrencyId",
                table: "JobOfferEmploymentDetails",
                column: "SalaryCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferEmploymentDetails_SalaryFromRangeId",
                table: "JobOfferEmploymentDetails",
                column: "SalaryFromRangeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobOfferEmploymentDetails");

            migrationBuilder.DropTable(
                name: "CurrencyTypes");

            migrationBuilder.DropTable(
                name: "EmploymentTypes");

            migrationBuilder.DropTable(
                name: "JobOfferSalariesRange");

            migrationBuilder.DropTable(
                name: "JobOffers");
        }
    }
}
