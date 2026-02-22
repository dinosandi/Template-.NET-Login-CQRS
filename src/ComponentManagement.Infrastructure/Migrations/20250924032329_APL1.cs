using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComponentManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class APL1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bagian",
                table: "APLParts");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "APLParts");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "APLParts");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "APLParts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "APLParts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "APLParts");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "APLParts");

            migrationBuilder.AddColumn<string>(
                name: "Bagian",
                table: "APLParts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "APLParts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "APLParts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
