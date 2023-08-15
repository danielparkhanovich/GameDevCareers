using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Offer_Plans_JobOffers_PlanId_Property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_JobOfferPlans_PlanId",
                table: "JobOffers");

            migrationBuilder.AlterColumn<int>(
                name: "PlanId",
                table: "JobOffers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_JobOfferPlans_PlanId",
                table: "JobOffers",
                column: "PlanId",
                principalTable: "JobOfferPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_JobOfferPlans_PlanId",
                table: "JobOffers");

            migrationBuilder.AlterColumn<int>(
                name: "PlanId",
                table: "JobOffers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_JobOfferPlans_PlanId",
                table: "JobOffers",
                column: "PlanId",
                principalTable: "JobOfferPlans",
                principalColumn: "Id");
        }
    }
}
