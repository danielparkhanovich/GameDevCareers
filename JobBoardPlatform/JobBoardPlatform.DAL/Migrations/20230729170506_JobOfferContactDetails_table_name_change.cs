using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class JobOfferContactDetails_table_name_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactDetails_ContactTypes_ContactTypeId",
                table: "ContactDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_ContactDetails_ContactDetailsId",
                table: "JobOffers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ContactDetails",
                table: "ContactDetails");

            migrationBuilder.RenameTable(
                name: "ContactDetails",
                newName: "JobOfferContactDetails");

            migrationBuilder.RenameIndex(
                name: "IX_ContactDetails_ContactTypeId",
                table: "JobOfferContactDetails",
                newName: "IX_JobOfferContactDetails_ContactTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobOfferContactDetails",
                table: "JobOfferContactDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOfferContactDetails_ContactTypes_ContactTypeId",
                table: "JobOfferContactDetails",
                column: "ContactTypeId",
                principalTable: "ContactTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_JobOfferContactDetails_ContactDetailsId",
                table: "JobOffers",
                column: "ContactDetailsId",
                principalTable: "JobOfferContactDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOfferContactDetails_ContactTypes_ContactTypeId",
                table: "JobOfferContactDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_JobOfferContactDetails_ContactDetailsId",
                table: "JobOffers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobOfferContactDetails",
                table: "JobOfferContactDetails");

            migrationBuilder.RenameTable(
                name: "JobOfferContactDetails",
                newName: "ContactDetails");

            migrationBuilder.RenameIndex(
                name: "IX_JobOfferContactDetails_ContactTypeId",
                table: "ContactDetails",
                newName: "IX_ContactDetails_ContactTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContactDetails",
                table: "ContactDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactDetails_ContactTypes_ContactTypeId",
                table: "ContactDetails",
                column: "ContactTypeId",
                principalTable: "ContactTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_ContactDetails_ContactDetailsId",
                table: "JobOffers",
                column: "ContactDetailsId",
                principalTable: "ContactDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
