using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComponentManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Revisi_Historical : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Historicals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PartComponentId = table.Column<Guid>(type: "uuid", nullable: false),
                    TanggalRFU = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OldCodeNumber = table.Column<string>(type: "text", nullable: false),
                    TanggalInstall = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NewCodeNumber = table.Column<string>(type: "text", nullable: false),
                    Hm = table.Column<string>(type: "text", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historicals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historicals_Components_PartComponentId",
                        column: x => x.PartComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Historicals_PartComponentId",
                table: "Historicals",
                column: "PartComponentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Historicals");
        }
    }
}
