using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class renamed_main_field_to_main_technology_Offer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_MainFieldTypes_MainFieldTypeId",
                table: "JobOffers");

            migrationBuilder.DropTable(
                name: "MainFieldTypes");

            migrationBuilder.RenameColumn(
                name: "MainFieldTypeId",
                table: "JobOffers",
                newName: "MainTechnologyTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_JobOffers_MainFieldTypeId",
                table: "JobOffers",
                newName: "IX_JobOffers_MainTechnologyTypeId");

            migrationBuilder.CreateTable(
                name: "MainTechnologyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainTechnologyTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MainTechnologyTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Programming" },
                    { 2, "Audio" },
                    { 3, "Graphics3D" },
                    { 4, "LevelDesign" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_MainTechnologyTypes_MainTechnologyTypeId",
                table: "JobOffers",
                column: "MainTechnologyTypeId",
                principalTable: "MainTechnologyTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_MainTechnologyTypes_MainTechnologyTypeId",
                table: "JobOffers");

            migrationBuilder.DropTable(
                name: "MainTechnologyTypes");

            migrationBuilder.RenameColumn(
                name: "MainTechnologyTypeId",
                table: "JobOffers",
                newName: "MainFieldTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_JobOffers_MainTechnologyTypeId",
                table: "JobOffers",
                newName: "IX_JobOffers_MainFieldTypeId");

            migrationBuilder.CreateTable(
                name: "MainFieldTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainFieldTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "MainFieldTypes",
                columns: new[] { "Id", "Type" },
                values: new object[,]
                {
                    { 1, "Programming" },
                    { 2, "Audio" },
                    { 3, "Graphics3D" },
                    { 4, "LevelDesign" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_MainFieldTypes_MainFieldTypeId",
                table: "JobOffers",
                column: "MainFieldTypeId",
                principalTable: "MainFieldTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
