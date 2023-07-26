using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Offer_Replaced_TechKeywords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOfferEmploymentDetails_JobOfferSalariesRange_SalaryRangeId",
                table: "JobOfferEmploymentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TechKeywords_JobOffers_JobOfferId",
                table: "TechKeywords");

            migrationBuilder.DropIndex(
                name: "IX_TechKeywords_JobOfferId",
                table: "TechKeywords");

            migrationBuilder.DropIndex(
                name: "IX_TechKeywords_Name",
                table: "TechKeywords");

            migrationBuilder.DropColumn(
                name: "JobOfferId",
                table: "TechKeywords");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TechKeywords",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "SalaryRangeId",
                table: "JobOfferEmploymentDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "JobOfferTechKeywords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobOfferId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOfferTechKeywords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobOfferTechKeywords_JobOffers_JobOfferId",
                        column: x => x.JobOfferId,
                        principalTable: "JobOffers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferTechKeywords_JobOfferId",
                table: "JobOfferTechKeywords",
                column: "JobOfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOfferEmploymentDetails_JobOfferSalariesRange_SalaryRangeId",
                table: "JobOfferEmploymentDetails",
                column: "SalaryRangeId",
                principalTable: "JobOfferSalariesRange",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOfferEmploymentDetails_JobOfferSalariesRange_SalaryRangeId",
                table: "JobOfferEmploymentDetails");

            migrationBuilder.DropTable(
                name: "JobOfferTechKeywords");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TechKeywords",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "JobOfferId",
                table: "TechKeywords",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SalaryRangeId",
                table: "JobOfferEmploymentDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TechKeywords_JobOfferId",
                table: "TechKeywords",
                column: "JobOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_TechKeywords_Name",
                table: "TechKeywords",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOfferEmploymentDetails_JobOfferSalariesRange_SalaryRangeId",
                table: "JobOfferEmploymentDetails",
                column: "SalaryRangeId",
                principalTable: "JobOfferSalariesRange",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechKeywords_JobOffers_JobOfferId",
                table: "TechKeywords",
                column: "JobOfferId",
                principalTable: "JobOffers",
                principalColumn: "Id");
        }
    }
}
