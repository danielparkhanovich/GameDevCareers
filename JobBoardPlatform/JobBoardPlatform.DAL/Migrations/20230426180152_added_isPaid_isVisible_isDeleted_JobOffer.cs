using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class added_isPaid_isVisible_isDeleted_JobOffer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBilled",
                table: "JobOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "JobOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "JobOffers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBilled",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "JobOffers");
        }
    }
}
