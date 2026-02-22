using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComponentManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RelasiComponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PartComponentId",
                table: "APLs",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_APLs_PartComponentId",
                table: "APLs",
                column: "PartComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_APLs_Components_PartComponentId",
                table: "APLs",
                column: "PartComponentId",
                principalTable: "Components",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APLs_Components_PartComponentId",
                table: "APLs");

            migrationBuilder.DropIndex(
                name: "IX_APLs_PartComponentId",
                table: "APLs");

            migrationBuilder.DropColumn(
                name: "PartComponentId",
                table: "APLs");
        }
    }
}
