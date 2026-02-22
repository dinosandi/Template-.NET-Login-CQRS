using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComponentManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fieldpriorty1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartComponentAPLs_Components_PartComponentId",
                table: "PartComponentAPLs");

            migrationBuilder.AddForeignKey(
                name: "FK_PartComponentAPLs_Components_PartComponentId",
                table: "PartComponentAPLs",
                column: "PartComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartComponentAPLs_Components_PartComponentId",
                table: "PartComponentAPLs");

            migrationBuilder.AddForeignKey(
                name: "FK_PartComponentAPLs_Components_PartComponentId",
                table: "PartComponentAPLs",
                column: "PartComponentId",
                principalTable: "Components",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
