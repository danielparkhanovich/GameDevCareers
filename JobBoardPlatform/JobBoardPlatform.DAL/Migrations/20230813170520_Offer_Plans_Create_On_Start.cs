using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Offer_Plans_Create_On_Start : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshedOnPageAt",
                table: "JobOffers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "JobOfferCategoryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOfferCategoryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobOfferPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicationDaysCount = table.Column<int>(type: "int", nullable: false),
                    EmploymentLocationsCount = table.Column<int>(type: "int", nullable: false),
                    OfferRefreshesCount = table.Column<int>(type: "int", nullable: false),
                    IsAbleToRedirectApplications = table.Column<bool>(type: "bit", nullable: false),
                    FreeSlotsCount = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOfferPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobOfferPlans_JobOfferCategoryTypes_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "JobOfferCategoryTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "JobOfferCategoryTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Employment" },
                    { 2, "Commissions" }
                });

            migrationBuilder.InsertData(
                table: "JobOfferPlans",
                columns: new[] { "Id", "CategoryId", "EmploymentLocationsCount", "FreeSlotsCount", "IsAbleToRedirectApplications", "OfferRefreshesCount", "PublicationDaysCount" },
                values: new object[,]
                {
                    { 1, 2, 1, 0, true, 1, 30 },
                    { 2, 1, 3, 0, false, 3, 30 },
                    { 3, 1, 10, 0, true, 7, 45 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferPlans_CategoryId",
                table: "JobOfferPlans",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobOfferPlans");

            migrationBuilder.DropTable(
                name: "JobOfferCategoryTypes");

            migrationBuilder.DropColumn(
                name: "RefreshedOnPageAt",
                table: "JobOffers");
        }
    }
}
