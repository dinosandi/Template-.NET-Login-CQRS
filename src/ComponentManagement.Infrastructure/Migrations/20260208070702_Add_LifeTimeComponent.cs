using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComponentManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_LifeTimeComponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComponentLifetimes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PartComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalLifetimeHm = table.Column<double>(type: "double precision", nullable: false),
                    InstalledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsedLifetimeHm = table.Column<double>(type: "double precision", nullable: false),
                    RemainingLifetimeHm = table.Column<double>(type: "double precision", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentLifetimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComponentLifetimes_Components_PartComponentId",
                        column: x => x.PartComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentLifetimes_PartComponentId",
                table: "ComponentLifetimes",
                column: "PartComponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentLifetimes");
        }
    }
}
