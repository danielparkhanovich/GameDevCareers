using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class JobOfferPlans_Additional_Properties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NameId",
                table: "JobOfferPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PriceInPLN",
                table: "JobOfferPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "JobOfferPlanTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOfferPlanTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "JobOfferPlanTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "COMMISSION" },
                    { 2, "INDIE" },
                    { 3, "AAA" }
                });

            migrationBuilder.UpdateData(
                table: "JobOfferPlans",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "NameId", "PriceInPLN" },
                values: new object[] { 1, 25 });

            migrationBuilder.UpdateData(
                table: "JobOfferPlans",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FreeSlotsCount", "NameId", "PriceInPLN" },
                values: new object[] { 50, 2, 50 });

            migrationBuilder.UpdateData(
                table: "JobOfferPlans",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "NameId", "PriceInPLN" },
                values: new object[] { 3, 125 });

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferPlans_NameId",
                table: "JobOfferPlans",
                column: "NameId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOfferPlans_JobOfferPlanTypes_NameId",
                table: "JobOfferPlans",
                column: "NameId",
                principalTable: "JobOfferPlanTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOfferPlans_JobOfferPlanTypes_NameId",
                table: "JobOfferPlans");

            migrationBuilder.DropTable(
                name: "JobOfferPlanTypes");

            migrationBuilder.DropIndex(
                name: "IX_JobOfferPlans_NameId",
                table: "JobOfferPlans");

            migrationBuilder.DropColumn(
                name: "NameId",
                table: "JobOfferPlans");

            migrationBuilder.DropColumn(
                name: "PriceInPLN",
                table: "JobOfferPlans");

            migrationBuilder.UpdateData(
                table: "JobOfferPlans",
                keyColumn: "Id",
                keyValue: 2,
                column: "FreeSlotsCount",
                value: 0);
        }
    }
}
