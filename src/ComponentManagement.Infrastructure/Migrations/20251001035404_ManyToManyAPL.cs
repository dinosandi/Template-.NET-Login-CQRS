using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComponentManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyAPL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PartComponentAPLs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PartComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    APLId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartComponentAPLs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartComponentAPLs_APLs_APLId",
                        column: x => x.APLId,
                        principalTable: "APLs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartComponentAPLs_Components_PartComponentId",
                        column: x => x.PartComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartComponentAPLs_APLId",
                table: "PartComponentAPLs",
                column: "APLId");

            migrationBuilder.CreateIndex(
                name: "IX_PartComponentAPLs_PartComponentId",
                table: "PartComponentAPLs",
                column: "PartComponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartComponentAPLs");
        }
    }
}
