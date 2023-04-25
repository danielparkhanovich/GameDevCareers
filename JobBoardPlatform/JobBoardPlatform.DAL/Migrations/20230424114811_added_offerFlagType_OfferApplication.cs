using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class added_offerFlagType_OfferApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationFlagTypeId",
                table: "OfferApplications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ApplicationFlagTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationFlagTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ApplicationFlagTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Default" },
                    { 2, "MustHire" },
                    { 3, "AverageHire" },
                    { 4, "RejectHire" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfferApplications_ApplicationFlagTypeId",
                table: "OfferApplications",
                column: "ApplicationFlagTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OfferApplications_ApplicationFlagTypes_ApplicationFlagTypeId",
                table: "OfferApplications",
                column: "ApplicationFlagTypeId",
                principalTable: "ApplicationFlagTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OfferApplications_ApplicationFlagTypes_ApplicationFlagTypeId",
                table: "OfferApplications");

            migrationBuilder.DropTable(
                name: "ApplicationFlagTypes");

            migrationBuilder.DropIndex(
                name: "IX_OfferApplications_ApplicationFlagTypeId",
                table: "OfferApplications");

            migrationBuilder.DropColumn(
                name: "ApplicationFlagTypeId",
                table: "OfferApplications");
        }
    }
}
