using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComponentManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RelasiComponent1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_APLs_Components_PartComponentId",
                table: "APLs");

            migrationBuilder.AlterColumn<Guid>(
                name: "PartComponentId",
                table: "APLs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<Guid>(
                name: "PartComponentId",
                table: "APLs",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_APLs_Components_PartComponentId",
                table: "APLs",
                column: "PartComponentId",
                principalTable: "Components",
                principalColumn: "Id");
        }
    }
}
