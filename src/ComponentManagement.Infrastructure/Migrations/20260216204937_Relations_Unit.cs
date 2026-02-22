using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComponentManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Relations_Unit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UnitId",
                table: "ComponentLifetimes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComponentLifetimes_UnitId",
                table: "ComponentLifetimes",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComponentLifetimes_Units_UnitId",
                table: "ComponentLifetimes",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComponentLifetimes_Units_UnitId",
                table: "ComponentLifetimes");

            migrationBuilder.DropIndex(
                name: "IX_ComponentLifetimes_UnitId",
                table: "ComponentLifetimes");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "ComponentLifetimes");
        }
    }
}
