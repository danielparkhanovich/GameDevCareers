using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class refactored_members_Offer_added_work_location_type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkLocationType",
                table: "JobOffers",
                newName: "Address");

            migrationBuilder.AddColumn<int>(
                name: "WorkLocationId",
                table: "JobOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WorkLocationsTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkLocationsTypes", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "EmploymentTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Type",
                value: "MandateContract");

            migrationBuilder.InsertData(
                table: "WorkLocationsTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "OnSite" },
                    { 2, "Hybrid" },
                    { 3, "FullyRemote" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_WorkLocationId",
                table: "JobOffers",
                column: "WorkLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_WorkLocationsTypes_WorkLocationId",
                table: "JobOffers",
                column: "WorkLocationId",
                principalTable: "WorkLocationsTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_WorkLocationsTypes_WorkLocationId",
                table: "JobOffers");

            migrationBuilder.DropTable(
                name: "WorkLocationsTypes");

            migrationBuilder.DropIndex(
                name: "IX_JobOffers_WorkLocationId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "WorkLocationId",
                table: "JobOffers");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "JobOffers",
                newName: "WorkLocationType");

            migrationBuilder.UpdateData(
                table: "EmploymentTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Type",
                value: "MandatoryContract");
        }
    }
}
