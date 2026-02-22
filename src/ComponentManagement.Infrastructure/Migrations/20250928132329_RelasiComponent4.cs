using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComponentManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RelasiComponent4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APLs_Components_PartComponentId",
                table: "APLs");

            migrationBuilder.AddForeignKey(
                name: "FK_APLs_Components_PartComponentId",
                table: "APLs",
                column: "PartComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APLs_Components_PartComponentId",
                table: "APLs");

            migrationBuilder.AddForeignKey(
                name: "FK_APLs_Components_PartComponentId",
                table: "APLs",
                column: "PartComponentId",
                principalTable: "Components",
                principalColumn: "Id");
        }
    }
}
