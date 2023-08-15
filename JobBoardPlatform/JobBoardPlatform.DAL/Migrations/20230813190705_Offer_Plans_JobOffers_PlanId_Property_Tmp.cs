using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Offer_Plans_JobOffers_PlanId_Property_Tmp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "JobOffers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_PlanId",
                table: "JobOffers",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_JobOfferPlans_PlanId",
                table: "JobOffers",
                column: "PlanId",
                principalTable: "JobOfferPlans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_JobOfferPlans_PlanId",
                table: "JobOffers");

            migrationBuilder.DropIndex(
                name: "IX_JobOffers_PlanId",
                table: "JobOffers");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "JobOffers");
        }
    }
}
