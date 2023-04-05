using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoardPlatform.DAL.Migrations
{
    /// <inheritdoc />
    public partial class renamed_CompanyCredentials_and_EmployeeCredentials_to_CompanyIdentity_and_EmployeeIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyCredentials");

            migrationBuilder.DropTable(
                name: "EmployeeCredentials");

            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "EmployeeProfiles",
                newName: "ProfileImageUrl");

            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                table: "CompanyProfiles",
                newName: "ProfileImageUrl");

            migrationBuilder.CreateTable(
                name: "CompanyIdentities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HashPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyIdentities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyIdentities_CompanyProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "CompanyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeIdentities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HashPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeIdentities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeIdentities_EmployeeProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "EmployeeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyIdentities_Email",
                table: "CompanyIdentities",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyIdentities_ProfileId",
                table: "CompanyIdentities",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeIdentities_Email",
                table: "EmployeeIdentities",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeIdentities_ProfileId",
                table: "EmployeeIdentities",
                column: "ProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyIdentities");

            migrationBuilder.DropTable(
                name: "EmployeeIdentities");

            migrationBuilder.RenameColumn(
                name: "ProfileImageUrl",
                table: "EmployeeProfiles",
                newName: "PhotoUrl");

            migrationBuilder.RenameColumn(
                name: "ProfileImageUrl",
                table: "CompanyProfiles",
                newName: "PhotoUrl");

            migrationBuilder.CreateTable(
                name: "CompanyCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HashPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyCredentials_CompanyProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "CompanyProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HashPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeCredentials_EmployeeProfiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "EmployeeProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCredentials_Email",
                table: "CompanyCredentials",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyCredentials_ProfileId",
                table: "CompanyCredentials",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCredentials_Email",
                table: "EmployeeCredentials",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCredentials_ProfileId",
                table: "EmployeeCredentials",
                column: "ProfileId");
        }
    }
}
