using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class SoftwareNameUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Softwares_Name",
                table: "Softwares",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Softwares_Name",
                table: "Softwares");
        }
    }
}
