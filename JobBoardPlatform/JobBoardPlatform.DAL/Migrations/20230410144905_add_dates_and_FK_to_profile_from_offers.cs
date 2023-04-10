using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class add_dates_and_FK_to_profile_from_offers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyProfileId",
                table: "JobOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "JobOffers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "JobOffers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "JobOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Published",
                table: "JobOffers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_CompanyProfileId",
                table: "JobOffers",
                column: "CompanyProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_CompanyProfiles_CompanyProfileId",
                table: "JobOffers",
                column: "CompanyProfileId",
                principalTable: "CompanyProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_CompanyProfiles_CompanyProfileId",
                table: "JobOffers");

            migrationBuilder.DropIndex(
                name: "IX_JobOffers_CompanyProfileId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "CompanyProfileId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "Published",
                table: "JobOffers");
        }
    }
}
