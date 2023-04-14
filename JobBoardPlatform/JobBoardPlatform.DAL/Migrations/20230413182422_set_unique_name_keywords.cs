using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class set_unique_name_keywords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechKeyWords_JobOffers_JobOfferId",
                table: "TechKeyWords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TechKeyWords",
                table: "TechKeyWords");

            migrationBuilder.RenameTable(
                name: "TechKeyWords",
                newName: "TechKeywords");

            migrationBuilder.RenameIndex(
                name: "IX_TechKeyWords_JobOfferId",
                table: "TechKeywords",
                newName: "IX_TechKeywords_JobOfferId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TechKeywords",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TechKeywords",
                table: "TechKeywords",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TechKeywords_Name",
                table: "TechKeywords",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TechKeywords_JobOffers_JobOfferId",
                table: "TechKeywords",
                column: "JobOfferId",
                principalTable: "JobOffers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TechKeywords_JobOffers_JobOfferId",
                table: "TechKeywords");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TechKeywords",
                table: "TechKeywords");

            migrationBuilder.DropIndex(
                name: "IX_TechKeywords_Name",
                table: "TechKeywords");

            migrationBuilder.RenameTable(
                name: "TechKeywords",
                newName: "TechKeyWords");

            migrationBuilder.RenameIndex(
                name: "IX_TechKeywords_JobOfferId",
                table: "TechKeyWords",
                newName: "IX_TechKeyWords_JobOfferId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TechKeyWords",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TechKeyWords",
                table: "TechKeyWords",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TechKeyWords_JobOffers_JobOfferId",
                table: "TechKeyWords",
                column: "JobOfferId",
                principalTable: "JobOffers",
                principalColumn: "Id");
        }
    }
}
