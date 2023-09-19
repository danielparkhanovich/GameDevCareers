using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MainTechnologyTypes_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "JobOfferPlans",
                keyColumn: "Id",
                keyValue: 1,
                column: "OfferRefreshesCount",
                value: 0);

            migrationBuilder.UpdateData(
                table: "JobOfferPlans",
                keyColumn: "Id",
                keyValue: 2,
                column: "OfferRefreshesCount",
                value: 1);

            migrationBuilder.UpdateData(
                table: "JobOfferPlans",
                keyColumn: "Id",
                keyValue: 3,
                column: "OfferRefreshesCount",
                value: 3);

            migrationBuilder.InsertData(
                table: "MainTechnologyTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 5, "Testing" },
                    { 6, "Management" },
                    { 7, "Other" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MainTechnologyTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MainTechnologyTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MainTechnologyTypes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "JobOfferPlans",
                keyColumn: "Id",
                keyValue: 1,
                column: "OfferRefreshesCount",
                value: 1);

            migrationBuilder.UpdateData(
                table: "JobOfferPlans",
                keyColumn: "Id",
                keyValue: 2,
                column: "OfferRefreshesCount",
                value: 3);

            migrationBuilder.UpdateData(
                table: "JobOfferPlans",
                keyColumn: "Id",
                keyValue: 3,
                column: "OfferRefreshesCount",
                value: 7);
        }
    }
}
