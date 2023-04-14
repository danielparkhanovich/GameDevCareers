using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class added_ContactDetails_and_ContactType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContactDetailsId",
                table: "JobOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ContactTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactTypeId = table.Column<int>(type: "int", nullable: false),
                    ContactAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactDetails_ContactTypes_ContactTypeId",
                        column: x => x.ContactTypeId,
                        principalTable: "ContactTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ContactTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Mail" },
                    { 2, "ExternalForm" },
                    { 3, "PrivateApplications" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_ContactDetailsId",
                table: "JobOffers",
                column: "ContactDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactDetails_ContactTypeId",
                table: "ContactDetails",
                column: "ContactTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_ContactDetails_ContactDetailsId",
                table: "JobOffers",
                column: "ContactDetailsId",
                principalTable: "ContactDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_ContactDetails_ContactDetailsId",
                table: "JobOffers");

            migrationBuilder.DropTable(
                name: "ContactDetails");

            migrationBuilder.DropTable(
                name: "ContactTypes");

            migrationBuilder.DropIndex(
                name: "IX_JobOffers_ContactDetailsId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "ContactDetailsId",
                table: "JobOffers");
        }
    }
}
