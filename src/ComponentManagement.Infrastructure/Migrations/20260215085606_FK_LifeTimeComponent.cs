using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComponentManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FK_LifeTimeComponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Units_UnitId",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Units");

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Units_UnitId",
                table: "Components",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Components_Units_UnitId",
                table: "Components");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Units",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Units",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Components_Units_UnitId",
                table: "Components",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
