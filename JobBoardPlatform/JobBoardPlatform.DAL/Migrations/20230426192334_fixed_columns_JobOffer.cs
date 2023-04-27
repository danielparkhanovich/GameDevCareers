using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class fixed_columns_JobOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsVisible",
                table: "JobOffers",
                newName: "IsSuspended");

            migrationBuilder.RenameColumn(
                name: "IsBilled",
                table: "JobOffers",
                newName: "IsShelved");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "JobOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "JobOffers");

            migrationBuilder.RenameColumn(
                name: "IsSuspended",
                table: "JobOffers",
                newName: "IsVisible");

            migrationBuilder.RenameColumn(
                name: "IsShelved",
                table: "JobOffers",
                newName: "IsBilled");
        }
    }
}
